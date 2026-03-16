# Simulación de Cola en Restaurante – Proyecto Unity

## 1. Descripción del proyecto

Este proyecto es una simulación básica de un sistema de colas en un restaurante o cafetería desarrollada en **Unity**.

La simulación representa:

* llegada de clientes
* formación de una fila
* atención en un cajero
* salida de los clientes después del servicio

El objetivo es modelar un sistema simple de colas usando lógica de programación y conceptos de simulación.

Tecnologías utilizadas:

* Unity (motor de desarrollo)
* C#
* Git / GitHub para control de versiones

---

# 2. Estructura del repositorio

El repositorio contiene principalmente estas carpetas:

Assets/
Packages/
ProjectSettings/

Carpetas importantes:

Assets
Contiene todo lo que usamos en la simulación:

* scripts
* modelos
* prefabs
* escenas

Packages
Dependencias del proyecto.

ProjectSettings
Configuración del proyecto de Unity.

Archivos ignorados por Git:

Library/
Temp/
Logs/

Estas carpetas se generan automáticamente cuando Unity abre el proyecto.

---

# 3. Requisitos para trabajar en el proyecto

Antes de comenzar necesitas instalar:

Unity Hub
Unity (misma versión del proyecto)
Git

---

# 4. Cómo descargar el proyecto

Clonar el repositorio desde GitHub:

git clone https://github.com/USUARIO/REPOSITORIO.git

Luego abrir **Unity Hub**:

Add Project
Seleccionar la carpeta descargada.

Unity generará automáticamente las carpetas que faltan.

---

# 5. Cómo ejecutar la simulación

1. Abrir la escena principal dentro de:

Assets/Scenes

2. Presionar:

Play

La simulación realizará lo siguiente:

* los clientes aparecen en el spawn
* forman una cola
* avanzan hacia el cajero
* reciben servicio
* salen del restaurante

---

# 6. Componentes principales del sistema

## SimulationManager

Script principal que controla:

* llegada de clientes
* cola
* servicio del cajero

Variables importantes:

minArrivalTime
tiempo mínimo entre llegadas

maxArrivalTime
tiempo máximo entre llegadas

serviceTime
tiempo de atención del cajero

maxQueueSize
tamaño máximo de la fila

## Customer

Controla el movimiento del cliente:

* desplazamiento hacia un objetivo
* movimiento en la fila
* salida del restaurante

## Puntos de referencia en la escena

spawnPoint
donde aparecen los clientes

queuePoints
posiciones de la fila

cashierPoint
posición del cajero

exitPoint
salida del restaurante

---

# 7. Sistema de simulación

La simulación sigue este flujo:

1. llega un cliente
2. entra en la cola
3. avanza si hay espacio
4. llega al cajero
5. recibe servicio
6. sale del restaurante

Los tiempos están basados en el ejercicio de simulación:

10 clientes por hora llegan al sistema

12 clientes por hora pueden ser atendidos

---

# 8. Prefabs

Los clientes están configurados como **prefabs** dentro de:

Assets/Prefabs

El sistema puede generar clientes aleatorios desde una lista de prefabs.

Esto permite usar distintos modelos de personajes.

---

# 9. Reglas para trabajar en equipo

Para evitar conflictos:

1. Hacer `pull` antes de empezar a trabajar

git pull

2. Hacer cambios pequeños y claros

3. Hacer commit con mensajes descriptivos

git commit -m "mejora movimiento clientes"

4. Subir cambios al repositorio

git push

---

# 10. Recomendaciones

No subir carpetas generadas por Unity:

Library
Temp
Logs

No modificar escenas si otra persona está trabajando en ellas al mismo tiempo.

Preferir dividir tareas como:

persona 1: scripts
persona 2: entorno 3D
persona 3: interfaz o animaciones

---

# 11. Posibles mejoras futuras

* mejorar animaciones de los clientes
* mejorar el entorno del restaurante
* agregar más cajeros
* agregar estadísticas de simulación
* mostrar tiempo promedio de espera
* agregar interfaz de control de la simulación

---

# 12. Estado actual del proyecto

Actualmente la simulación incluye:

✔ generación aleatoria de clientes
✔ sistema de cola
✔ atención en cajero
✔ salida de clientes
✔ modelos de personajes con animaciones básicas
✔ entorno inicial del restaurante

El sistema funciona correctamente como simulación básica.

---
