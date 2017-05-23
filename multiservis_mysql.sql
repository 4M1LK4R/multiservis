create database multiservis;
use multiservis;



create table area
(
id int auto_increment,
nombre varchar (50)not null,
estado bit not null,
primary key(id)
);


create table servicio
(
id int auto_increment,
area int not null,
nombre varchar (50)not null,
estado bit not null,
primary key(id),
index area_servicio(area),
    foreign key(area)
    references area(id)
);
