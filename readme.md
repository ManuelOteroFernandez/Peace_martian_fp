# Mecánicas del jugador

## **Primarias**

| Mecánica | Tipo | Necesidad | Reglas |
| --- | --- | --- | --- |
| Movimiento | Principal | \-Desplazamiento lateral<br><br>\-Salto<br><br>\-Dash | El jugador tendrá libertad de movimiento en todo momento, salvo en ciertas secuencias puntuales. |
| Combate | Principal | \-Disparo<br><br>\-Cambio de armas<br><br>\-Vida del jugador | En cada nivel, el jugador dispondrá de distintos elementos en su arsenal para combatir contra los enemigos. |

## Secundarias

| Mecánica | Tipo | Necesidad | Reglas |
| --- | --- | --- | --- |
| Salto | Secundaria | \-Doble salto | El jugador podrá saltar siempre y cuando se encuentre en tierra, y una vez más en el aire. |
| Dash | Secundaria | \-Inmunidad | El jugador podrá realizar un dash para desplazarse rápidamente. Durante este dash, el jugador tendrá inmunidad al daño y no podrá realizar ninguna otra acción hasta que esta finalice. |
| Cambio de armas | Secundaria | \-Objetos interactuables | El jugador tendrá la habilidad de cambiar entre las distintas armas disponibles en su arsenal en cualquier momento. |
| Disparo | Secundaria | \-Armas | Cada arma implementará un tipo distinto de disparo. Ciertas armas serán más útiles en algunas situaciones que otras. |
| Vida del jugador | Secundaria | \-Objetos interactuables | El jugador puede resistir 1 único golpe de los enemigos. Si el jugador encuentra cierto power-up repartido por el mapa, podrá acumular 1 vida extra, permitiendo que aguante 2 golpes de los enemigos antes de morir. |
| Armas | Secundaria |     | Existirán 3 tipos de armas, cada una con un comportamiento diferente y una interacción diferente con el medio. |

## Terciario

| Mecánica | Tipo | Necesidad | Reglas |
| --- | --- | --- | --- |
| Coleccionables | Terciaria |     | Existirán diversos coleccionables repartidos por el escenario que el jugador podrá recoger. |

### Mecánicas de desplazamiento

El jugador dispondrá de los siguientes movimientos para desplazarse por el mapa:

#### Desplazamiento lateral

El jugador podrá moverse de forma lateral por el mapa utilizando las teclas de movimiento asignadas o el joystick izquierdo de un controlador.

#### Salto

El jugador podrá saltar pulsando la tecla de salto asignada o el botón correspondiente del controlador. El salto funciona de la siguiente forma:

- El jugador puede saltar siempre que se encuentre en tierra.
- El jugador puede realizar 1 salto en el aire.

#### Dash

El jugador podrá realizar un dash pulsando la tecla asignada o el botón correspondiente del controlador. El funcionamiento del dash es el siguiente:

- El jugador puede realizar el dash tanto en tierra como en el aire.
- Mientras el jugador está realizando el dash no puede realizar ninguna otra acción (salto, cambio de arma, disparo).
- El jugador obtiene un momento de invulnerabilidad al daño mientras se encuentra realizando el dash.
- El jugador puede atravesar ciertos elementos (como enemigos) realizando el dash.
- El dash tiene un tiempo de reutilización determinado para evitar que el jugador esté continuamente en este estado.

### Tipos de armas

#### Pistola de burbujas

Se trata del arma básica del jugador. Esta arma se encuentra disponible desde el inicio del juego, y acompañará al jugador hasta el final.

El funcionamiento del arma es el siguiente:

- Cuando el jugador dispara el arma se lanza un proyectil en forma de burbuja.
- La burbuja puede desplazarse cierta distancia antes de explotar.
- Si la burbuja choca contra el terreno explotará.
- Cuando la burbuja choca contra un enemigo se quedará pegada a él.
- Cuando cierto número de burbujas se encuentren pegadas a un enemigo, se transformarán en una burbuja más grande que atrapará al enemigo y empezará a flotar.
- Cada enemigo requiere un número de burbujas pequeñas para quedar incapacitado.

#### Cañón de burbujas

Esta arma se desbloquea en cierto punto del juego. El funcionamiento es muy similar a su hermana pequeña, la Pistola de burbujas. Las diferencias son las siguientes:

- Arma con una cadencia de fuego mucho menor que la Pistola de burbujas.
- Las burbujas de esta arma son mucho más grandes que las de la Pistola de burbujas.
- Esta arma puede incapacitar directamente a la mayoría de enemigos con un único disparo.

#### Cañón de agua

La última arma que podrá obtener el jugador se trata de un Cañón de agua. Esta arma dispara un chorro ininterrumpido de agua a presión. El funcionamiento es el siguiente:

- Para que esta arma funcione correctamente, el jugador tiene que mantener pulsado el botón de disparo.
- Mientras el jugador mantenga presionado el botón de disparo, el arma disparará un chorro de agua continuo.
- El chorro de agua tiene una distancia máxima.
- El chorro de agua solo dura una cantidad determinada de tiempo antes de que el arma se sobrecaliente. Cuando esto ocurre, el jugador tendrá que esperar un poco antes de poder volver a disparar.
- El chorro de agua empujará a los enemigos.
- Esta arma no puede incapacitar a los enemigos como si lo hacen la Pistola y el Cañón de burbujas.

# Mecánicas del sistema

## Procesos del sistema

| Mecánica | Necesidades | Reglas |
| --- | --- | --- |
| Puntos de control |     | Cuando el jugador alcance ciertos puntos preestablecidos en el mapa, el sistema creará un punto de control, permitiendo al jugador comenzar desde ese lugar en caso de muerte. |
| Sistema de alarma | \-IA y sistema de pathfinding. | En ciertos lugares del mapa existirán diferentes balizas que los enemigos podrán activar. Si una de estas balizas se activa, se irán generando enemigos hasta que el jugador destruya dicha baliza. |

## Procesos de personaje

| Mecánica | Necesidades | Reglas |
| --- | --- | --- |
| Enemigos | \-IA y sistema de pathfinding y seguimiento. | Existirán 3 tipos diferentes de enemigos que atacarán al jugador, cada uno con un patrón de comportamiento diferente. |

### Tipos de enemigos

El jugador se enfrentará a 3 tipos distintos de enemigos. Estos enemigos tendrán diferentes características.

#### Soldado cuerpo a cuerpo

- Este enemigo atacará cuerpo a cuerpo al jugador. Se desplazará a donde el jugador para poder atacar y lo perseguirá en todo momento.

#### Soldado a distancia

- Este enemigo dispara balas al enemigo desde la distancia. Si el jugador se acerca mucho intentará alejarse de él buscando una posición desde la que seguir disparando.
- Este tipo de enemigo también puede disparar a las burbujas del jugador para intentar liberar a sus aliados incapacitados.

#### Soldado acorazado

- Los enemigos ya saben como luchas, y se adaptan a tus habilidades. Este tipo de enemigos llevarán un escudo con púas que hará que las burbujas del jugador exploten y no surtan efecto.
- Existen variantes cuerpo a cuerpo y variantes a distancia de este tipo de enemigo.
- El jugador tendrá que disparar al enemigo por la espalda o desarmarlo /temporalmente utilizando su Cañón de agua.

## Comportamiento de enemigos (IA)

Todos los enemigos tienen en común los siguientes comportamientos:

- **Detección del jugador**: Los enemigos tienen un radio de detección del jugador, este radio puede ser variable.
- **Patrulla:** Los enemigos que no detecten al jugador, pueden tener una patrulla asociada, esto son 2 puntos entre los que el enemigo se mueve constantemente. En caso de no tener este comportamiento permanecen quietos.
- **Persecución:** una vez detectado el jugador, los enemigos intentarán acercarse al jugador para atacarlo. Existen puntualizaciones por cada tipo de enemigo.
- **Activar alarma:** Es posible que el enemigo tenga una alarma asociada. Si este es el caso, cuando el enemigo detecta al jugador se moverá hasta la alarma y la activará.
- **Escalada:** Para cambiar de altura, los enemigos pueden usar escaleras. Mientras los enemigos están en las escalando, no pueden atacar.
- **Atrapado en burbuja**: Si el jugador atrapa al enemigo en una burbuja, **este flotará** hasta chocar un el techo o será destruido por collider para evitar que choque hasta el infinito.  
    Esta burbuja **puede ser explotada por otro enemigo a distancia** o por pinchos situados en el entrono.

### Enemigos a distancia

#### Movimiento

Estos enemigos **no pueden alejarse mucho de su posición de patrulla.**

Solo saltarán alturas de 1 unidad (un tile) el resto de alturas tendrán que bajarlas usando escaleras.

Para subir a una mayor altura necesitan escaleras.

#### Persecución

Los enemigos a distancia intentan **mantener su posición**, **se acercan al jugador si lo detectaron pero sin alejarse una cantidad X de su posición de patrulla.**

En el caso de que el jugador se aleje lo suficiente, y el enemigo no pueda seguir persiguiéndole (por la distancia a su posición de patrulla), **el enemigo volverá a su posición de patrulla**.

#### Ataque

Los enemigos a distancias atacarán al jugador si lo detectan disparando proyectiles.

Si detectan a otro enemigo atrapado en una burbuja, tendrán una probabilidad de atacar la burbuja para salvar al enemigo atrapado.

Tienen una candencia de disparo X.


# Narrativa

## Idea

¡El planeta Tierra está en peligro!

Un valiente marciano ha viajado desde las estrellas para traer un mensaje urgente, pero los humanos, temerosos y desconfiados, responden con fuerza militar antes de escucharle. A pesar de los obstáculos, nuestro visitante extraterrestre no se rendirá. Con determinación y su ingenioso cañón de burbujas, se abrirá paso entre las filas de soldados hasta llegar al presidente.

## Logline

¡La Tierra está en peligro, ábrete paso entre los ataques de los desconfiados humanos para advertir al presidente antes de que sea demasiado tarde!

## Sinopsis

¡Prepárate para una experiencia única en el mundo de los videojuegos! _Peace Martian_ es un innovador juego de acción que da un giro pacifista a los enfrentamientos. En lugar de recurrir a la violencia, tu misión como un marciano defensor de la paz es abrirte paso a través del ejército español utilizando tu ingenioso cañón de burbujas. Cada burbuja calma a tus oponentes, desmontando el conflicto de una manera creativa y estratégica.

Tu objetivo final: llegar hasta el presidente y demostrar que el entendimiento y la paz son más poderosos que la guerra. Con una jugabilidad dinámica y un mensaje profundamente positivo, _Peace Martian_ redefine la acción en los videojuegos, dejando una huella inolvidable en los jugadores. ¡La paz nunca fue tan divertida!

## Arquetipo

La narrativa del juego se dividirá en **3 actos:**

- **Planteamiento:** Se cuenta como el marciano descubre el problema de la tierra, su llegada a la tierra y como los humanos se muestran agresivos hacia el.
- **Nudo:** En esta parte se cuenta como nuestro protagonista se abre paso “pacíficamente” entre las tropas humanas hasta llegar al presidente
- **Desenlace:** El final de la historia es simple, el marciano consigue llegar al presidente, pero este no le hace caso, así que decide marcharse de la tierra y no ayudarlos. La tierra explota (Causa aun sin concretar).

## Estructura narrativa

Estructura linear 🙂

## Elementos adicionales

Al adquirir ciertos objetos podemos descubrir algo más sobre la historia del marciano:

- **Coleccionables:** El jugador puede encontrar escondidos por el mapa varios coleccionables. Estos darán pistas o complementarán la historia de la comunidad marciana.
- **Armas:** Al adquirir nuevas armas además de explicar cómo funcionan aprendes información sobre su origen y cómo fueron creadas por los marcianos.

# Personajes

## El marciano

**Personaje principal**

Datos generales:

- **Nombre**: No tiene
- **Raza**: Marciano
- **Edad**: Mediana edad

**Aspecto**: Piel roja, cabeza grande, estatura media

**Ropa**: Traje de astronauta

**Arma**: Cañón de burbujas, estilo lanzacohetes, ametralladora

**Accesorio**: Mochila grande de propulsión

**Valores**: Bondadoso, amable, protector

**Defectos**: Orgulloso, necio

Un **misterioso marciano** de **piel roja** y **gran cabeza** ha llegado a la Tierra con una misión crucial: advertir a la humanidad de un inminente peligro. A pesar de su apariencia extraterrestre, es **bondadoso**, **amable** y **protector**, dispuesto a arriesgarlo todo para cumplir su deber. Sin embargo, su **orgullo** y **terquedad** a veces lo llevan a **tomar decisiones impulsivas**.

Vestido con un resistente **traje de astronauta**, lleva consigo un **cañón de burbujas** con la potencia de un lanzacohetes o una ametralladora, diseñado para **calmar a sus oponentes sin dañarlos**. Su gran **mochila propulsora** le permite moverse con agilidad, sorteando obstáculos en su camino hacia el presidente.

## Presidente

**Personaje antagonista**

Datos generales:

- **Nombre**: Pedro Pérez (?)
- **Raza**: Humano
- **Nacionalidad**: Española
- **Edad**: Mediana edad

**Aspecto**: Varón, mayor, raza blanca, pelo corto, pulcro

**Ropa**: Traje

**Valores**: Protector, solidario, perseverante

**Defectos**: Orgulloso, necio, autoritario, incompetencia, actitud defensiva

**Pedro Pérez** es un **hombre** de **mediana edad**, siempre **impecable** en su **traje** bien planchado y con un **aire de autoridad** que impone respeto. Su **cabello corto** y su **expresión seria** reflejan su **compromiso con la seguridad de su nación**. Como líder militar o funcionario de alto rango en España, su misión es clara: **proteger a su pueblo de cualquier amenaza, sin importar su origen**.

Es un hombre **perseverante y solidario con los suyos**, dispuesto a tomar decisiones difíciles en tiempos de crisis. Sin embargo, su **orgullo** y **terquedad** lo hacen reaccionar de manera **impulsiva** y **defensiva ante lo desconocido**. Su **visión rígida del mundo**, combinada con su **tendencia autoritaria**, lo lleva a ver al marciano como un enemigo antes de intentar comprender su mensaje.

A pesar de su incompetencia estratégica en esta situación, **su actitud obstinada lo convierte en un obstáculo formidable en el camino del protagonista**. Mientras el marciano busca la paz, Pedro Pérez está dispuesto a detenerlo a toda costa, convencido de que está haciendo lo correcto… aunque quizá, en el fondo, tema estar equivocado.

## Soldados (Enemigos)

**NPCs hostiles**

Datos generales:

- **Nombre**: Variados
- **Raza**: Humano
- **Nacionalidad**: Española
- **Edad**: Variadas

**Aspecto**: Variado, militar, raza blanca, enfadado

**Ropa**: Uniforme militar

**Valores**: Obedecer, proteger, luchar

**Defectos**: Ignorantes, agresivos, toscos, cerrados

Los soldados no saben bien por qué deben luchar contra el marciano, solo saben que deben hacerlo, y el hecho de que no sepan nada de él y no sea como ellos les ayuda bastante a cumplir con su misión.

No tienen una motivación clara, solo quieren cumplir con las órdenes que les han dado e irse de vuelta a casa, sus opiniones sobre el marciano son indiferentes o negativas, pero solamente porque les han dicho que deben verlo así.

# Nivel 1 — Tutorial

Este nivel transcurre en un entrono urbano, pero a las afueras de una ciudad en el presente. Para ser más específicos, el nivel es en las afueras de Madrid, durante un día soleado, en un lugar con pocos edificios, lleno de barricadas y militares.

El objetivo de este nivel es que el jugador se familiarice con las mecánicas básicas del juego y tener una primera experiencia gratificante.

## Mecánicas

- Movimiento
  - Desplazamiento lateral
  - Salto
- Combate
  - Pistola de burbujas
  - Apuntar y disparar
  - Soldado a distancia (Enemigo básico)

## Objetivos y obstáculos

### Tutorial desplazamiento lateral

El primer objetivo es que el jugador aprenda a moverse, para ello le mostramos una imagen de como hacerlo a modo de tutorial y le dejamos un espacio seguro para que pruebe.

### Tutorial salto

El segundo objetivo es que el jugador aprenda a saltar, para ello le mostramos una imagen de como hacerlo y le ponemos pequeños saltos para que se familiarice con las distancias.

### Tutorial disparo y apuntado con pistola de burbujas

El 3º objetivo es que el jugador **aprenda a apuntar y disparar**, igual que hicimos anteriormente, le explicamos los controles con una imagen y se le prepara un entorno que le permite apuntar y disparar en 3 direcciones de forma segura (los enemigos no podrán verle si se mantiene en la cima)

### Tutorial disparo hacia abajo + salto

El 4º objetivo es enseñar al jugador que puede disparar mientras salta, para ello se preparó un primer combate en el cual el jugador tiene la ventaja de la altura y la sorpresa. El enemigo no detectará el jugador hasta que este se le acerque mucho.

### Enfrentamientos

A partir de este punto, el objetivo es que el jugador pruebe el combate, para ello se prepararon varios enfrentamientos, cada uno un poco más complejo que el anterior.

####

#### Enfrentamiento básico

Al enfrentarse con 2 enemigos, el jugador descubrirá que los enemigos se pueden apoyar entre sí.

#### Enfrentamiento con altura

En este enfrentamiento, se añade un enemigo en la altura, esto complica el apuntado y añade más ángulos de ataque al jugador.

#### Enfrentamiento final

Este es el último enfrentamiento y se me mezclan todo lo aprendido anteriormente.

## Referencias

El nivel tiene como referencia los primeros niveles de Metal Slug en los que podemos encontrar enemigos distraídos que funcionan más como saco de entrenamiento que como enemigos.  

## Lista de assets

- ~~Imágenes explicativas~~
  - ~~Movimiento~~
  - ~~Salto~~
  - ~~Disparo y apuntado~~
  - ~~Salto y disparo~~
  - ~~Doble salto~~
  - ~~Dash~~
  - ~~Subir escaleras~~
- ~~Cubos de basura~~
- Barandillas
- ~~Señales de tráfico~~
- Fondo
- Entorno
- Animaciones de enemigos
- ~~Balas (Enemigos)~~
- ~~Burbujas~~
- ~~Burbuja explota~~
- ~~Animación Burbuja con enemigo explota~~

## Lista de secretos

Para darle rejugabilidad, conseguir que el nivel no sea tan lineal y sea un poco más complejo hay que añadir.

- Caminos secundarios
- Coleccionables

## Resultado

Después de varias iteraciones, el resultado del nivel es el siguiente:

[Peace_Martian_Tutorial_V1_Gameplay.mp4](https://drive.google.com/file/d/1J9eyGK93ECVjWqdwWqr6pQUvuaMsNkbF/view?usp=sharing)

[Peace_Martian_Tutorial_V2_Gameplay.mp4](https://drive.google.com/file/d/1RiyB3tU7s2e92MeGRQ-2utCxA3_iI9Kf/view?usp=sharing)

# Nivel 2 -

## Mecánicas

- Movimiento
  - Dash
  - Doble salto
- Combate
  - Escudo de burbuja
  - Soldado a distancia (Enemigo básico)

# Nivel 3 -

# Sonido

## Sonidos DX

Ninguno

## Sonidos MX

- [Música del menú principal](https://drive.google.com/file/d/1d9BQdJKbLepzbJ-CFDuBRfosDEOFA-u_/view?usp=drive_link)
- Música del nivel 1 (pendiente de componer)
- Música del nivel 2 (pendiente de componer)
- Música de pantalla de muerte (pendiente de componer)

## Sonidos SFX

- Disparos enemigos
- Pasos enemigos
- Explosión burbujas
- Aterrizaje de los enemigos en el suelo
- Ataques a melee de los enemigos
- Hover de opciones de los menús
- Activación de opciones de los menús
- Activación de alarma

## Sonidos FOL

- Pasos del jugador
- Dash
- Salto
- Cambio de arma
- Disparo

## Sonidos BG

- Sonido de helicópteros
- Sonido de coches

Todos los sonidos (incluidos SFX y DX) serán implementados en una fuente de audio 2D, ya que se intenta simular un juego árcade donde este tipo de fuente de audio era más prevalente.

Como nuestro protagonista no puede hablar español, no existen diálogos en el juego, por lo que no se incorporará ningún sonido DX. La prioridad de sonidos será la siguiente:

| **Tipo de sonido** | **Prioridad** |
| --- | --- |
| MX  | 200 |
| SFX | 120 |
| FOL | 140 |
| BG  | 100 |

# Testing

[Plan de pruebas en hoja de cálculo.](https://docs.google.com/spreadsheets/d/1sK1NvBplb6kOELzMBzVABRnKZx1daZcCDKvEWge6pyk/edit?gid=0#gid=0)

## Pruebas funcionales

<table><thead><tr><th><p>ID</p></th><th><p>Descripción</p></th><th><p>Resultado esperado</p></th></tr><tr><th><p>F1</p></th><th><p>El jugador puede moverse horizontalmente</p></th><th><p>El personaje responde correctamente a los controles y se mueve horizontalmente</p></th></tr><tr><th><p>F2</p></th><th><p>El jugador salta con el botón de salto</p></th><th><p>El jugador se mueve verticalmente y cae al alcanzar cierta altura.</p></th></tr><tr><th><p>F3</p></th><th><p>El doble salto funciona solo una vez en el aire</p></th><th><p>El jugador vuelve a saltar una única vez más cuando se pulsa el botón de salto estando en el aire.</p></th></tr><tr><th><p>F4</p></th><th><p>El jugador realiza un dash</p></th><th><p>Al pulsar el botón de dash, el jugador se mueve rápidamente una corta distancia en la dirección en la que se está desplazando</p></th></tr><tr><th><p>F5</p></th><th><p>El jugador puede disparar su arma</p></th><th><p>El jugador dispara proyectiles que se desplazan en la dirección en la que se está apuntando y que caen lentamente.</p></th></tr><tr><th><p>F6</p></th><th><p>El jugador puede cambiar de arma</p></th><th><p>El arma equipada del jugador cambia cuando se pulsan los botones de cambio de arma, siempre y cuando tenga más de un arma en su posesión.</p></th></tr><tr><th><p>F7</p></th><th><p>Los ataques enemigos dañan al jugador</p></th><th><ul><li>Si el jugador tiene armadura: Recibir un ataque destruye la armadura del jugador.</li><li>Si el jugador no tiene armadura: Recibir un ataque hace que el jugador muera.</li></ul></th></tr><tr><th><p>F8</p></th><th><p>Los enemigos reciben ataques del jugador</p></th><th><p>Cuando un proyectil del jugador impacta en un enemigo, dicho proyectil se queda pegado al enemigo.</p></th></tr><tr><th><p>F9</p></th><th><p>Los enemigos quedan atrapados al recibir X número de proyectiles</p></th><th><p>Cuando un enemigo recibe varios proyectiles del jugador queda atrapado en una burbuja que flota y se desplaza hacia arriba.</p></th></tr><tr><th><p>F10</p></th><th><p>Los enemigos atrapados no realizan acciones</p></th><th><p>Cuando un enemigo está atrapado no puede realizar ninguna acción como perseguir al jugador, atacar, etc.</p></th></tr><tr><th><p>F11</p></th><th><p>Los enemigos mantienen su posición</p></th><th><p>Cuando los enemigos se encuentran lejos del jugador, se encuentran estáticos o bien patrullan un área determinada.</p></th></tr><tr><th><p>F12</p></th><th><p>Los enemigos persiguen al jugador</p></th><th><p>Cuando el jugador se acerca a cierta distancia de un enemigo, este se desplaza en dirección al jugador.</p></th></tr><tr><th><p>F13</p></th><th><p>Los enemigos a distancia no saltan grandes distancias</p></th><th><p>Los enemigos a distancia solo pueden saltar pequeños salientes. Si la distancia de caída es muy grande, el enemigo se mantiene al borde del saliente esperando/atacando al jugador.</p></th></tr><tr><th><p>F14</p></th><th><p>Los enemigos no se alejan de su posición de patrulla</p></th><th><p>Los enemigos solo se alejan cierta distancia de su posición inicial de patrulla. Si se alejan demasiado, dan vuelta y siguen patrullando.</p></th></tr><tr><th><p>F15</p></th><th><p>Los enemigos a distancia disparan si tienen visión del objetivo</p></th><th><p>Los enemigos a distancia disparan proyectiles al jugador siempre que se encuentre a cierta distancia y exista una línea de disparo entre el enemigo y el jugador.</p></th></tr><tr><th><p>F16</p></th><th><p>Los enemigos a melee se acercan al jugador para atacar</p></th><th><p>Los enemigos a melee persiguen al jugador hasta estar muy cerca de él y lanzan un ataque de corta distancia.</p></th></tr><tr><th><p>F17</p></th><th><p>Reaparición en checkpoints</p></th><th><p>Si el jugador muere, reaparece en el último checkpoint que ha activado.</p></th></tr><tr><th><p>F18</p></th><th><p>Recogida de objetos</p></th><th><p>Cuando el jugador se acerca lo suficiente a un objeto interactuable, este objeto desaparece y aplica las bonificaciones oportunas al jugador (rellena la armadura, desbloquea arma o desbloquea una habilidad)</p></th></tr><tr><th><p>F19</p></th><th><p>Finalización del nivel</p></th><th><p>El jugador finaliza el nivel una vez llega a cierto punto del mismo.</p></th></tr></thead></table>

## Smoke tests

<table><thead><tr><th><p>ID</p></th><th><p>Descripción</p></th><th><p>Resultado esperado</p></th></tr><tr><th><p>S1</p></th><th><p>El juego se inicia</p></th><th><p>El juego se inicia correctamente, sin ninguna traza de error en los logs y sin ningún cuelgue detectable por el usuario.</p></th></tr><tr><th><p>S2</p></th><th><p>La UI responde</p></th><th><p>El usuario puede desplazarse por la interfaz de usuario y acceder a todas las opciones disponibles.</p></th></tr><tr><th><p>S3</p></th><th><p>El nivel se inicia</p></th><th><p>Cuando el usuario accede a las opciones para iniciar una nueva partida, el juego carga correctamente el nivel oportuno.</p></th></tr><tr><th><p>S4</p></th><th><p>Los controles responden</p></th><th><p>El usuario puede realizar correctamente todas las acciones básicas disponibles para el jugador:</p><ul><li>Movimiento horizontal</li><li>Salto</li><li>Disparo</li></ul></th></tr><tr><th><p>S5</p></th><th><p>Los enemigos se cargan correctamente</p></th><th><p>El jugador puede visualizar a los enemigos y estos responden al comportamiento esperado (patrulla, persecución, ataque, retirada)</p></th></tr></thead></table>

## Pruebas A/B

| ID  | Descripción | A   | B   | Métrica |
| --- | --- | --- | --- | --- |
| AB1 | ¿El número de armas afecta a la retención? | 2 armas | 3 armas | Retención diaria, tasa de abandono |
| AB2 | ¿Más acción o plataformas? | Niveles orientados a la acción | Niveles orientados al plataformeo | Tiempo medio jugado, retención diaria. |
| AB3 | ¿Velocidad de juego más alta o más baja? | Velocidad de movimiento/proyectiles/enemigos lenta | Velocidad de movimiento/proyectiles/enemigos alta | Tiempo medio jugado. |
| AB4 | ¿Interfaz minimalista o sobrecargada? | Interfaz minimalista | Interfaz sobrecargada | Feedback de usuario. |

## Pruebas de rendimiento

| ID  | Descripción | Resultado esperado |
| --- | --- | --- |
| R1  | Pruebas en PC gama baja | El juego mantiene >= 60FPS en equipos de gama baja |
| R2  | Pruebas en navegador | El juego mantiene >= 30FPS en navegadores de uso habitual |
| R3  | Tiempos de carga | El tiempo de carga de un nivel es inferior a 5s. |

## Test unitarios

Para la realización de los test unitarios se hará uso de Unity Test Framework.

| ID  | Método testeado | Descripción |
| --- | --- | --- |
| U1  | PlayerController.Move() | Comprueba que el movimiento lateral del jugador responde correctamente. |
| U2  | PlayerController.Jump() | Comprueba que el jugador puede saltar y se cumplen con las restricciones del doble salto |
| U3  | PlayerController.Dash() | Comprueba que el jugador puede realizar un dash y que se respeta el cooldown del mismo. |
| U4  | PlayerController.Attack() | Comprueba que el jugador puede disparar y que se respeta la cadencia de fuego establecida. |
| U5  | EnemyController.Move() | Comprueba que el enemigo se puede mover correctamente por el escenario. |
| U6  | EnemyController.Attack() | Comprueba que el enemigo puede realizar ataques correctamente. |

# Requerimientos técnicos

- **Plataforma/s objetivo:** Windows, Web
- **Motor de juego:** Unity
- **Lenguaje de programación:** C#
- **Requerimientos de hardware:** Windows o Acceso a internet, ratón y teclado o mando
- Herramientas de software
  - **IDE:** VS Code
  - **Herramientas:** Inkscape
- **Consideraciones de input:** Teclado y mouse o mando
- **Sistema de guardado:** Local

# Lanzamiento

## Monetización

Para monetizar nuestro videojuego optaremos por un sistema de crowdfunding a través de Patreon. Esta plataforma permite a los jugadores apoyar el proyecto con aportaciones mensuales, lo que facilita una financiación continua sin depender de inversores externos. A cambio, los patrocinadores podrán acceder a contenido exclusivo, versiones anticipadas y votaciones sobre futuras mejoras.

## Distribución

La distribución se hará mediante Itchio y Steam, ya que ofrecen buena visibilidad, herramientas para desarrolladores independientes y canales integrados de comunicación con los usuarios.  
Itchio nos permitirá testear el juego de forma más libre durante las primeras fases, ideal para recoger feedback temprano. Steam, por otro lado, será el canal principal para el lanzamiento completo, por su gran base de usuarios y sistema de reseñas.

Además, aprovecharemos las funciones de acceso anticipado de Steam para generar comunidad desde etapas tempranas del desarrollo.

## Plan de parches y mejoras

En cuanto al plan de parches y mejoras, se prevé implementar las siguientes acciones tras el lanzamiento:

- Corrección de errores tras la beta, centrados en bugs críticos y problemas reportados por los testers.  

- Optimización del rendimiento en dispositivos menos potentes, ajustando texturas, físicas y carga de memoria.  

- Nuevas funciones y contenidos según el feedback de la comunidad, como modos de juego adicionales, mejoras de accesibilidad o ajustes de dificultad.  

- Aplicación de prácticas LiveOps, como actualizaciones regulares para mantener la atención del jugador y fomentar la participación continua.

# Marketing

## Transformación de la demo en producto final

Para convertir nuestra demo en un producto completo, seguiremos una hoja de ruta basada en hitos de desarrollo. Actualmente, contamos con un prototipo jugable que presenta las mecánicas principales y la estética del juego. El siguiente paso será el desarrollo de una versión _alfa_, en la que estarán integrados todos los sistemas esenciales, aunque sin el nivel de pulido final. Posteriormente, lanzaremos una versión _beta_, centrada en la corrección de errores, balance de mecánicas, optimización y pruebas de rendimiento. Esta versión servirá como base para un lanzamiento en acceso anticipado, en el que recogeremos feedback directo de la comunidad para aplicar las últimas mejoras antes del estreno oficial. El objetivo es llegar a una versión final sólida, estable y con un nivel de calidad que cumpla con las expectativas del público.  

## Simulación de marketing y mercado objetivo

Nuestra campaña de marketing se basará en el modelo AIDA, siguiendo las fases de Atención, Interés, Deseo y Acción. Esto implica generar visibilidad del proyecto desde sus primeras etapas y acompañar al jugador a lo largo del proceso de desarrollo, fortaleciendo su vínculo con el producto.

El juego está orientado a un público joven, en un rango amplio de entre 8 y 25 años, que busca experiencias accesibles, visualmente atractivas y fáciles de entender, pero con suficiente profundidad como para mantener el interés. Este perfil de jugador consume habitualmente contenido en redes sociales, YouTube, TikTok y plataformas móviles, y responde positivamente a estímulos visuales llamativos y comunicación cercana.

Adaptaremos, por tanto, el lenguaje, el estilo y los canales de la campaña a estos hábitos de consumo, con una imagen de marca divertida, colorida y amigable.

## Vías de publicitación del videojuego

- **Redes sociales:** presencia constante en TikTok, Instagram y Twitter/X mediante clips cortos de gameplay, publicaciones del desarrollo, encuestas interactivas y contenido generado por la comunidad. Se priorizarán formatos dinámicos y visuales que enganchen a los usuarios desde los primeros segundos.  

- **Campaña de email marketing:** aprovecharemos la demo publicada en Itchio para invitar a los jugadores a registrarse a una newsletter. A través del correo, enviaremos actualizaciones del desarrollo, fechas de lanzamiento, contenido exclusivo y acceso anticipado a ciertas versiones.  

- **Colaboración con influencers y prensa especializada:** se establecerán contactos con creadores de contenido enfocados al público joven (especialmente en YouTube y Twitch), a quienes se les facilitarán versiones anticipadas del juego. Además, se enviarán notas de prensa y kits promocionales a medios digitales especializados en videojuegos independientes y cultura juvenil.