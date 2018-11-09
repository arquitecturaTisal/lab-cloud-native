export default class Mensaje {
  usuario: string = '';
  mensaje: string = '';
  fecha: string = '';
  mio: boolean;

  constructor(usuario: string, mensaje: string, fecha: string, mio : boolean) {
    this.usuario = usuario;
    this.mensaje = mensaje;
    this.fecha = fecha;
    this.mio = mio;
  }
}
