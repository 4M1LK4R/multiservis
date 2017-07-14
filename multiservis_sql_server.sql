use master;

create database multiservis;

use multiservis;

create table area(
id int identity(1,1) not null primary key,
nombre varchar (50)not null,
estado bit not null
);

create table servicio(
id int identity(1,1) not null primary key,
area int not null,
nombre varchar (50)not null,
estado bit not null,
constraint area_servicio
    foreign key(area)
    references area(id)
);

create table tipo_servicio(
id int identity(1,1) not null primary key,
nombre varchar(50) not null,
estado bit not null
);

create table rol(
id int identity(1,1) not null primary key,
nombre varchar(50) not null,
estado bit not null
);

create table privilegio(
id int identity(1,1) not null primary key,
rol int not null,
nombre varchar(50) not null,
estado bit not null,
constraint rol_privilegio
    foreign key(rol)
    references rol(id)
);

create table persona(
id int identity(1,1) not null primary key,
ci int not null,
nombres varchar(50) not null,
paterno varchar(50),
materno varchar(50),
correo varchar(50),
nacionalidad varchar(50) not null,
telefono varchar(50),
direccion varchar(50)
);

/*
create table telefono(
  id int identity(1,1) not null primary key,
  persona int not null,
  tipo varchar(50) not null,
  numero int not null,
  estado bit not null,
  constraint persona_telefono(persona),
      foreign key(persona)
      references persona(id)
);

create table direccion(
  id int identity(1,1) not null primary key,
  persona int not null,
  direccion_fija varchar(80) not null,
  zona varchar(50) not null,
  barrio varchar(50) not null,
  calle varchar(50)not null,
  estado bit not null,
  constraint persona_direccion(persona),
      foreign key(persona)
      references persona(id)
);*/

create table usuario(
    id int identity(1,1) not null primary key,
    persona int not null,
    nombre_usuario varchar(50) not null,
    pasword_usuario varchar(50) not null,
    estado bit not null,
    constraint persona_usuario
      foreign key(persona)
      references persona(id)
);

create table asignar_rol_usuario(
    id int identity(1,1) not null primary key,
    rol int not null,
    usuario int not null,
    fecha_asigna date not null,
    estado bit not null,
    constraint rol_asignar_rol_usuario
        foreign key (rol)
        references rol(id),
    constraint usuario_asignar_rol_usuario
        foreign key (usuario)
        references usuario(id)
);

create table tecnico(
  id int identity(1,1) not null primary key,
  ruta_imagen varchar(max),
  persona int not null,
  nro_seguro int not null,
  salario decimal(8,2) not null,
  fecha_inscripcion date not null,
  estado bit not null,
  constraint persona_tecnico
      foreign key(persona)
      references persona(id)
);

create table tecnico_area(
    id int identity(1,1) not null primary key,
    tecnico int not null,
    tipo_servicio int not null,
    fecha date not null,
    especialidad varchar(50) not null,
    nivel varchar(50) not null,
    estado bit not null,
    constraint tecnico_tecnico_area
        foreign key(tecnico)
        references tecnico(id),
    constraint tipo_servicio_tecnico_area
        foreign key(tipo_servicio)
        references tipo_servicio(id)        
);
create table material(
    id int identity(1,1) not null primary key,
    nombre varchar(50) not null,
    id_servicio int not null
);
create table unidad_material(
    id int identity(1,1) not null primary key,
    material int not null,
    precio_compra decimal(8,2) not null,
    precio_venta decimal(8,2) not null,
    fecha_ingreso date not null,
    estado bit not null,
    constraint material_unidad_material
        foreign key(material)
        references material(id)
);
create table herramienta(
    id int identity(1,1) not null primary key,
    nombre varchar(50) not null,
    id_servicio int not null
);
create table unidad_herramienta(
    id int identity(1,1) not null primary key,
    herramienta int not null,
    precio_compra decimal(8,2) not null,
    precio_venta decimal(8,2) not null,
    fecha_ingreso date not null,
    estado bit not null,
    constraint herramienta_unidad_herramienta
        foreign key(herramienta)
        references herramienta(id)
);

create table detalle_servicio(
    id int identity(1,1) not null primary key,
    servicio int not null,    
    nombre varchar(50) not null,
    precio decimal(8,2) not null,
    tiempo varchar(50) not null,
    estado bit not null,
    constraint servicio_detalle_servicio
        foreign key(servicio)
        references servicio(id)
);

create table reserva(
    id int identity(1,1) not null primary key,
    fecha date,
    persona int,
    usuario int,
    monto_total decimal(8,2),
    estado bit not null,
    constraint persona_reserva
        foreign key(persona)
        references persona(id)
);

create table detalle_reserva(
    id int identity(1,1) not null primary key,
    reserva int,
    detalle_servicio int,
    tecnico int,
    usuario int,
    precio decimal(8,2),
    progreso varchar(20),
    descripcion varchar(50),
    estado bit not null,
    constraint reserva_detalle_reserva
        foreign key(reserva)
        references reserva(id),
    constraint detalle_servicio_detalle_reserva
        foreign key(detalle_servicio)
        references detalle_servicio(id),
    constraint tecnico_detalle_reserva
        foreign key(tecnico)
        references tecnico(id)     
);
create table control(
    id int identity(1,1) not null primary key,
    detalle_reserva int not null,
    fecha_inicio date,
    fecha_fin date,
    observacion varchar(50),
    estado bit not null,
    constraint detalle_reserva_control
        foreign key(detalle_reserva)
        references detalle_reserva(id)
);

create table ficha_tecnica(
    id int identity(1,1) not null primary key,
    detalle_reserva int,
    usuario_almacen int,
    nro_ficha int,
    descripcion_problema varchar(90),
    hora int,
    constraint detalle_reserva_ficha_tecnica
        foreign key(detalle_reserva)
        references detalle_reserva(id)
);

create table detalle_ficha_material(
    id int identity(1,1) not null primary key,
    ficha_tecnica int,
    unidad_material int,
    cantidad int,
    estado bit not null,
    constraint ficha_tecnica_detalle_ficha_material
        foreign key(ficha_tecnica)
        references ficha_tecnica(id),
    constraint unidad_material_detalle_ficha_material
        foreign key(unidad_material)
        references unidad_material(id)
);

create table detalle_ficha_herramienta(
    id int identity(1,1) not null primary key,
    ficha_tecnica int,
    unidad_herramienta int,
    cantidad int,
    estado bit not null,
    constraint ficha_tecnica_detalle_ficha_herramienta
        foreign key(ficha_tecnica)
        references ficha_tecnica(id),
    constraint unidad_herramienta_detalle_ficha_herramienta
        foreign key(unidad_herramienta)
        references unidad_herramienta(id)    
);
INSERT INTO rol ( nombre, estado) VALUES ( 'administrador', 1), ( 'estandar', 1);
INSERT INTO persona ( ci, nombres, paterno, materno, correo, nacionalidad, telefono, direccion) VALUES ( '1', 'admin', 'admin', 'admin', 'admin', 'admin', null, null);
INSERT INTO usuario ( persona, nombre_usuario, pasword_usuario, estado) VALUES ( '1', 'admin', 'admin', 1);
INSERT INTO asignar_rol_usuario ( rol, usuario, fecha_asigna, estado) VALUES ('1', '1', '2017-07-13', 1);
