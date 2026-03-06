import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule } from '@angular/common/http';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, FormsModule, HttpClientModule],
  templateUrl: './app.html', // Asegúrate que el archivo se llame app.html
  styleUrl: './app.css'      // Asegúrate que el archivo se llame app.css
})
export class AppComponent implements OnInit {
  // --- NAVEGACIÓN ---
  vistaActual: 'catalogo' | 'admin' | 'cargarServicio' | 'listaIncidentes' | 'login' = 'catalogo';

  // --- AUTENTICACIÓN ---
  modoRegistro = false;
  authData = { email: '', password: '', nombreCompleto: '' };
  usuarioLogueado: any = null;

  // --- PROPIEDADES ---
  propiedades: any[] = [];
  filtroTexto = '';
  filtroTipo = '';
  filtroOperacion = '';
  precioMin: number | null = null;
  precioMax: number | null = null;

  // --- FORMULARIO PROPIEDAD ---
  idEdicion: number | null = null;
  nuevaDireccion = '';
  nuevoPrecio = '';
  nuevaDescripcion = '';
  nuevaLocalidad = '';
  nuevaOperacion = 'Venta';
  nuevoTipoInmueble = 'Casa';
  nuevoAmbientes: number | null = null;

  // --- GESTIÓN DE INCIDENTES ---
  incidentes: any[] = [];
  propiedadSeleccionada: any = null;
  nuevoIncidente = {
    titulo: '',
    descripcion: '',
    urgencia: 'Media',
    oficio: 'Mantenimiento'
  };

  // URLs de API 
  private apiAuthUrl = 'https://localhost:7107/api/auth';
  private apiUrl = 'https://localhost:7107/api/propiedades';
  private apiPedidosUrl = 'https://localhost:7107/api/pedidos';

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.verificarSesion();
    this.obtenerPropiedades();
  }

  verificarSesion() {
    const sesion = localStorage.getItem('inmo_user');
    if (sesion) {
      try {
        this.usuarioLogueado = JSON.parse(sesion);
      } catch (e) {
        localStorage.removeItem('inmo_user');
      }
    }
  }

  cambiarVista(vista: any) {
    this.vistaActual = vista;
    this.propiedadSeleccionada = null;
    if (vista === 'listaIncidentes') {
      this.obtenerIncidentes();
    }
  }

  obtenerPropiedades() {
    this.http.get(this.apiUrl).subscribe({
      next: (data: any) => this.propiedades = data,
      error: (err) => console.error("Error API Propiedades:", err)
    });
  }

  guardarPropiedad() {
    if (!this.usuarioLogueado) {
      this.vistaActual = 'login';
      return;
    }

    const objetoPropiedad = {
      id: this.idEdicion || 0,
      direccion: this.nuevaDireccion,
      precio: this.nuevoPrecio,
      descripcion: this.nuevaDescripcion,
      localidad: this.nuevaLocalidad,
      tipoOperacion: this.nuevaOperacion,
      tipoInmueble: this.nuevoTipoInmueble,
      ambientes: this.nuevoAmbientes || 0,
      activo: true,
      usuarioId: this.usuarioLogueado.id
    };

    const request = this.idEdicion
      ? this.http.put(`${this.apiUrl}/${this.idEdicion}`, objetoPropiedad)
      : this.http.post(this.apiUrl, objetoPropiedad);

    request.subscribe({
      next: () => {
        alert("✅ Éxito");
        this.resetFormulario();
        this.obtenerPropiedades();
        this.vistaActual = 'catalogo';
      },
      error: () => alert("❌ Error")
    });
  }

  prepararEdicion(p: any) {
    this.idEdicion = p.id || p.Id;
    this.nuevaDireccion = p.direccion || p.Direccion;
    this.nuevoPrecio = p.precio || p.Precio;
    this.nuevaDescripcion = p.descripcion || p.Descripcion;
    this.nuevaLocalidad = p.localidad || p.Localidad;
    this.nuevaOperacion = p.tipoOperacion || p.TipoOperacion;
    this.nuevoTipoInmueble = p.tipoInmueble || p.TipoInmueble;
    this.nuevoAmbientes = p.ambientes || p.Ambientes;
    this.vistaActual = 'admin';
  }

  borrarPropiedad(id: number) {
    if (confirm('¿Eliminar?')) {
      this.http.delete(`${this.apiUrl}/${id}`).subscribe({
        next: () => this.obtenerPropiedades()
      });
    }
  }

  resetFormulario() {
    this.idEdicion = null;
    this.nuevaDireccion = '';
    this.nuevoPrecio = '';
    this.nuevaDescripcion = '';
    this.nuevaLocalidad = '';
    this.nuevaOperacion = 'Venta';
    this.nuevoTipoInmueble = 'Casa';
    this.nuevoAmbientes = null;
  }

  obtenerIncidentes() {
    this.http.get(this.apiPedidosUrl).subscribe({
      next: (data: any) => this.incidentes = data
    });
  }

  abrirReporte(p: any) {
    this.propiedadSeleccionada = p;
    this.vistaActual = 'listaIncidentes';
  }

  enviarIncidente() {
    const payload = {
      id: 0,
      titulo: this.nuevoIncidente.titulo,
      descripcion: this.nuevoIncidente.descripcion,
      urgencia: this.nuevoIncidente.urgencia,
      oficio: this.nuevoIncidente.oficio,
      fechaReporte: new Date().toISOString(),
      estado: 'Pendiente',
      propiedadId: this.propiedadSeleccionada.id || this.propiedadSeleccionada.Id,
      usuarioId: this.usuarioLogueado.id
    };

    this.http.post(this.apiPedidosUrl, payload).subscribe({
      next: () => {
        alert("✅ Reportado");
        this.propiedadSeleccionada = null;
        this.vistaActual = 'catalogo';
      }
    });
  }

  actualizarEstadoIncidente(incidente: any, nuevoEstado: string) {
    const actualizado = { ...incidente, estado: nuevoEstado };
    this.http.put(`${this.apiPedidosUrl}/${incidente.id || incidente.Id}`, actualizado).subscribe({
      next: () => this.obtenerIncidentes()
    });
  }

  cancelarReporte() {
    this.propiedadSeleccionada = null;
    this.vistaActual = 'catalogo';
  }

  ejecutarLogin() {
    this.http.post(`${this.apiAuthUrl}/login`, this.authData).subscribe({
      next: (res: any) => {
        this.usuarioLogueado = res;
        localStorage.setItem('inmo_user', JSON.stringify(res));
        this.vistaActual = 'catalogo';
        this.authData = { email: '', password: '', nombreCompleto: '' };
      },
      error: () => alert("Error de acceso")
    });
  }

  ejecutarRegistro() {
    const dataParaBackend = {
      id: 0,
      email: this.authData.email,
      passwordHash: this.authData.password,
      nombreCompleto: this.authData.nombreCompleto,
      rol: "Cliente",
      fechaRegistro: new Date().toISOString(),
      activo: true
    };
    this.http.post(`${this.apiAuthUrl}/registrar`, dataParaBackend).subscribe({
      next: () => {
        alert("✅ Registrado");
        this.modoRegistro = false;
        this.authData.password = '';
      }
    });
  }

  logout() {
    this.usuarioLogueado = null;
    localStorage.removeItem('inmo_user');
    this.vistaActual = 'catalogo';
  }

  limpiarFiltros() {
    this.filtroTexto = '';
    this.filtroTipo = '';
    this.filtroOperacion = '';
    this.precioMin = null;
    this.precioMax = null;
  }

  get propiedadesFiltradas() {
    return this.propiedades.filter(p => {
      const direccion = (p.direccion || p.Direccion || '').toLowerCase();
      const localidad = (p.localidad || p.Localidad || '').toLowerCase();
      const tipo = p.tipoInmueble || p.TipoInmueble || '';
      const operacion = p.tipoOperacion || p.TipoOperacion || '';
      const precioNum = parseInt((p.precio || p.Precio || '0').toString().replace(/[^0-9]/g, ''));

      return (direccion.includes(this.filtroTexto.toLowerCase()) || localidad.includes(this.filtroTexto.toLowerCase())) &&
        (this.filtroTipo === '' || tipo === this.filtroTipo) &&
        (this.filtroOperacion === '' || operacion === this.filtroOperacion) &&
        (this.precioMin === null || precioNum >= this.precioMin) &&
        (this.precioMax === null || precioNum <= this.precioMax);
    });
  }
}
