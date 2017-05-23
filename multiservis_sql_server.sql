
create database multiservis;
use multiservis;
create table area
(
id int idendity(1,1),
nombre varchar (50)not null,
estado bit not null,
primary key(id)
);


create table servicio
(
id int idendity(1,1),
area int not null,
nombre varchar (50)not null,
estado bit not null,
primary key(id),
index area_servicio
    foreign key(area)
    references area(id)
);
