# 2do Proyecto de Programación : Pixel Walle
## Liz Cartaya Salabarría
### C112

Pixel Walle es un programa que es capaz de recibir una serie de comandos del lenguaje Pixel_Walle-E y transformarlas en un dibujo en un canvas cuadriculado.

Esta serie de instrucciones siempre debe comenzar con:
## Spawn 
para mover a Wall-E (el pintor).
- Los comandos que forman parte de este lenguaje son (además del Spawn) :
## Color 
- Recibe un string y asigna el color.
- Los colores disponibles son:
  
 - Red,
 - Orange,
 - Blue,
 - White, (funciona como borrador puesto que el canvas es blanco inicialmente),
 - Transparent,
 - Purple,
 - Negro,
 - Yellow,
 - Green
  
  #### Adicionales:
  - Fushia,
  - Aqua
### Atención
  Tenga en cuenta que si no escribe los colores tal como están anteriormente el programa mostrará un error y no se ejecutará.

## Size
- Recibe un entero(int) positivo y mayor que cero y cambia el tamaño del pincel.

## DrawLine
- Recibe tres parámetros enteros ( dirección x, dirección y, cantidad de pasos)
- Puede realizarse en las ocho direcciones.
## DrawCircle
- Recibe tres parámetros enteros ( dirección x, dirección y, radio)

## DrawRectangle
- Recibe 5 parámetros enteros (dirección x, dirección y, distance, altura, ancho)

## Fill
- No recibe ningún parámetro.
- Revisa el color de la posición actual de Wall-E y pinta con el color actual del pincel los pixeles alcanzables sin caminar sobre otro color.
- Accede a los pixeles que son alcanzables visitando los de mayor profundidad.

### Adicionales

## DrawCuadrado
- Recibe 4 parámetros enteros (dirección x, dirección y, distance, tamaño)

## DrawAsterisco
- Recibe tres parámetros enteros ( dirección x, dirección y, tamaño)

## DrawRombo
- Recibe tres parámetros enteros ( dirección x, dirección y, tamaño)

## DrawTriangle
- Recibe 6 parámetros enteros 

## FillB
Al igual que el Fill
- No recibe ningún parámetro.
- Revisa el color de la posición actual de Wall-E y pinta con el color actual del pincel los pixeles alcanzables sin caminar sobre otro color.
  
Sin embargo, este:
- Accede a los pixeles que son alcanzables visitando primero los que se encuentran a menor distancia.

##### Además, existen otros comandos que brindan información 

## IsBrushColor
- Recibe un string color 
- Si el color que recibe es el que posee el pincel retorna 1, en caso contrario retorna 0.

## GetActualX
- No recibe ningún parámetro
- Retorna la posición en x de Wall-E

## GetActualY
- No recibe ningún parámetro
- Retorna la posición en y de Wall-E

## GetCanvasSize
- No recibe ningún parámetro
- Retorna el tamaño actual que posee el canvas

## IsBrushSize
- Recibe un entero  
- Si el valor que recibe es el size que posee el pincel retorna 1, en caso contrario retorna 0.

## IsCanvasColor
- Recibe tres parámetros ( string color, int x, int y)
- Revisa el color que posee el canvas en el pixel (x,y)
- Si el color del pixel coincide con el string color recibido retorna 1, en  caso contrario retorna 0.

## GetColorCount
- Recibe 5 parámetros (string color, int x, int y, int i, int j)
- Retorna la cantidad de pixeles del color recibido entre los pixeles (x,y) y (i,j).


### Además el lenguaje accepta 

## Asignacion de variables
- Cuenta con la sintaxis (identificador <- valor)

## Saltos condicionales
- Cuenta con la sintaxis: GoTO[etiqueta](condición)
- Una etiqueta es una cadena de texto que marca un lugar del código al cual se puede llegar a través de un  GoTo.
- Si al momento de leer el GoTo la condición se cumple se redirige a la línea donde se encuentra la etiqueta, en caso contrario se continua a la siguiente línea.

## Operaciones binarias
- Es capaz de resolver operaciones de :
  
- Adición(+)
- Sustracción(-)
- Multiplicación(*)
- Potencia(**)
- División(/)
- Módulo(%)

## Invocar funciones
-De esta manera se puede asignar el valor que retorna una función a una variable o realizar alguna operación.





  
  
