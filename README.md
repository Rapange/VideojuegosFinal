# VideojuegosFinal
Trabajo final de videojuegos

El juego consiste en una guerra entre dos tanques: el jugador controla al tanque azul y tiene que derrotar disparando al tanque rojo. Cada tanque solo puede soportar cuatro disparos antes de que se destruya y reaparezca en su respectivo Spawn.
La especificación se encuentra en el "Tank-Wars.pdf".
En la carpeta Assets se encuentran las carpetas utilizadas para la realización de este juego y le nombre de cada carpeta se explica por sí mismo. En la carpeta scripts se encuentran los scripts del juego:
- EnemyDetection: Permite que el enemigo detecte al jugador y activa ambas redes neuronales. Asimismo, desactiva ambas redes si deja de detectarlo.
- NeuralNetwork: Es la implementación de la red neuronal en sí. Es una red neuronal backpropagation con función de activación sigmoidal. Normalización min-max e inicialización de los pesos según la inicialización de Xavier.
- Bullet: Código de la bala, hace daño a los otros jugadores, se autodestruye al detectar colisión.
- collisionAvoid: Código para detectar la colisión con las paredes.
- enemy: Código del enemigo y su inteligencia. Aquí se inicializan ambas redes neuronales y también se pueden entrenar en el momento. Por ahora solo se entrena la red para apuntar, la otra red para moverse ya está entrenada y solo se inicializan los pesos. Aquí también está el comportamiento de "wandering". Una vez entrenadas, se le van pasando valores de entradas a estas redes y el enemigo reacciona según las salidas.
- master: Mantiene el score.
- player: Código del jugador.
- starter: Pasa a la siguiente escena.

La data con la cual ha sido entrenada ambas redes neuronales también se encuentran en esta carpeta:
- moveData: Data para entrenar la red neuronal de movimiento. Los tres primeros valores indican las entradas (ángulo que necesita rotar el jugador para aputar al enemigo, izquierda 0 o derecha 1, distancia entre tanque enemigo y jugador) y los cuatro últimos indican las salidas (0,1 arriba, abajo, derecha e izquierda).
- shootData: Data para entrenar la red neuronal de apuntar: Los dos primeros valores indican las entradas (ángulo que necesita rotar el enemigo para aputar al jugador, izquierda 0 o derecha 1) y el último indica las salida (0.0 rotar izquierda, 0.5 disparar, 1.0 rotar derecha).
