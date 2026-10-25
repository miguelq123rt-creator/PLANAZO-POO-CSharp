# PLANAZO - Sistema de eventos y reservas

## ¿De qué trata el proyecto?

PLANAZO es un programa desarrollado en C# que permite consultar eventos y reservar entradas desde la consola. También permite cancelar reservas y ver un resumen de las entradas reservadas y su valor total.

La idea del proyecto es poner en práctica los conceptos de programación orientada a objetos que hemos visto en clase, como las clases, la herencia, el polimorfismo y la encapsulación.

## Requisitos

Para ejecutar el programa se necesita tener instalado .NET 8 o una versión superior.

## ¿Cómo ejecutar el programa?

1. Abrir la carpeta del proyecto en Visual Studio Code.
2. Abrir una terminal.
3. Ejecutar el siguiente comando:

bash
dotnet run


Para ejecutar las pruebas del programa, se utiliza:

bash
dotnet run -- --pruebas


## ¿Qué se puede hacer en el programa?

El programa cuenta con varias opciones para administrar los eventos y sus reservas:

- Consultar los eventos disponibles.
- Ver la información de cada evento y su precio.
- Reservar entradas, teniendo en cuenta los cupos disponibles.
- Consultar las reservas realizadas.
- Cancelar una reserva y recuperar los cupos correspondientes.
- Ver un resumen de las reservas activas y su valor total.
- Consultar las reservas asociadas a un usuario.

También se realizan validaciones para evitar cantidades incorrectas, reservas que superen los cupos disponibles y cancelaciones repetidas.

El programa incluye películas, conciertos, talleres y un evento deportivo. Cada tipo de evento tiene su propia forma de calcular el precio final.

## Clases principales

Estas son las clases que se utilizaron para organizar el proyecto:

- **Evento:** es la clase base que contiene la información común de los eventos.
- **Cine:** representa los eventos relacionados con películas.
- **Concierto:** representa los conciertos y aplica un aumento del 15 % al precio base.
- **Taller:** representa los talleres y aplica un descuento del 10 %.
- **EventoDeportivo:** representa los eventos deportivos.
- **Reserva:** guarda la información de las entradas reservadas.
- **Usuario:** permite almacenar la información de los usuarios.
- **GestorReservas:** se encarga de administrar los eventos y las reservas.
- **Program:** contiene el menú principal y permite interactuar con el programa.

## Conceptos de programación orientada a objetos

Durante el desarrollo se aplicaron los siguientes conceptos:

- **Clases y objetos:** se utilizaron para representar los eventos, los usuarios y las reservas.
- **Encapsulación:** permite controlar el acceso y la modificación de los datos.
- **Herencia:** los diferentes tipos de eventos heredan las características de la clase `Evento`.
- **Abstracción:** la clase `Evento` define las características comunes sin representar