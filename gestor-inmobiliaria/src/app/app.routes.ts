import { Routes } from '@angular/router';

export const routes: Routes = [
  // Comenta estas líneas por ahora para que no interfieran con el diseño de app.html
  // { path: 'registro', component: RegistroComponent },

  // Deja la ruta raíz apuntando a nada o simplemente vacía
  { path: '', redirectTo: '', pathMatch: 'full' }
];
