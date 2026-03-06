import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterModule, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './registro.component.html',
  styleUrls: ['./registro.component.css']
})
export class RegistroComponent {
  nuevoUsuario = {
    nombreCompleto: '',
    email: '',
    password: '',
    rol: 'Cliente' // Rol por defecto
  };

  constructor(private authService: AuthService, private router: Router) { }

  registrar() {
    if (!this.nuevoUsuario.email || !this.nuevoUsuario.password) {
      alert("Por favor, completa los campos obligatorios.");
      return;
    }

    // Aquí llamarías a tu servicio authService.registrar(this.nuevoUsuario)
    console.log("Registrando usuario:", this.nuevoUsuario);
    // Ejemplo de redirección tras éxito:
    // this.router.navigate(['/login']);
  }
}
