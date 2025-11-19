DELETE FROM [RespuestasAlumnos];
DELETE FROM [Opciones];
DELETE FROM [Preguntas];
DELETE FROM [ExamenesAlumnos];
DELETE FROM [Inscripciones];
DELETE FROM [Examenes];
DELETE FROM [Cursos];
DELETE FROM [Alumnos];
DELETE FROM [Profesores];
DELETE FROM [Usuarios];
DELETE FROM [Materias];

DBCC CHECKIDENT ('[RespuestasAlumnos]', RESEED, 0);
DBCC CHECKIDENT ('[Opciones]', RESEED, 0);
DBCC CHECKIDENT ('[Preguntas]', RESEED, 0);
DBCC CHECKIDENT ('[ExamenesAlumnos]', RESEED, 0);
DBCC CHECKIDENT ('[Inscripciones]', RESEED, 0);
DBCC CHECKIDENT ('[Examenes]', RESEED, 0);
DBCC CHECKIDENT ('[Cursos]', RESEED, 0);
DBCC CHECKIDENT ('[Alumnos]', RESEED, 0);
DBCC CHECKIDENT ('[Profesores]', RESEED, 0);
DBCC CHECKIDENT ('[Usuarios]', RESEED, 0);
DBCC CHECKIDENT ('[Materias]', RESEED, 0);

SET IDENTITY_INSERT [Materias] ON;
INSERT INTO [Materias] (Id, Nombre, NotaDeAprobacion, NotaDePromocion) VALUES
(1, 'Programación I', 4.0, 7.0),
(2, 'Base de Datos II', 4.0, 7.0),
(3, 'Laboratorio III', 4.0, 7.0),
(4, 'Metodologías Ágiles', 4.0, 7.0);
SET IDENTITY_INSERT [Materias] OFF;

SET IDENTITY_INSERT [Usuarios] ON;
INSERT INTO [Usuarios] (Id, Username, Password, Nombre, Legajo) VALUES
(1, 'profesor_admin', 'admin123', 'Profesor Admin', 'P-001'),
(2, 'profesora_b', 'admin123', 'Profesora B', 'P-002'),
(3, 'ana_gomez', 'test123', 'Ana Gómez', 'A-101'),
(4, 'juan_perez', 'test123', 'Juan Pérez', 'A-102'),
(5, 'carlos_vega', 'test123', 'Carlos Vega', 'A-103'),
(6, 'lucia_diaz', 'test123', 'Lucía Díaz', 'A-104'),
(7, 'miguel_ruiz', 'test123', 'Miguel Ruiz', 'A-105'),
(8, 'sofia_chen', 'test123', 'Sofía Chen', 'A-201'),
(9, 'diego_lopez', 'test123', 'Diego López', 'A-202'),
(10, 'elena_moreno', 'test123', 'Elena Moreno', 'A-203'),
(11, 'martin_soto', 'test123', 'Martín Soto', 'A-204'),
(12, 'laura_silva', 'test123', 'Laura Silva', 'A-205');
SET IDENTITY_INSERT [Usuarios] OFF;

SET IDENTITY_INSERT [Profesores] ON;
INSERT INTO [Profesores] (Id, UsuarioId) VALUES
(1, 1),
(2, 2);
SET IDENTITY_INSERT [Profesores] OFF;

SET IDENTITY_INSERT [Alumnos] ON;
INSERT INTO [Alumnos] (Id, UsuarioId) VALUES
(1, 3),
(2, 4),
(3, 5),
(4, 6),
(5, 7),
(6, 8),
(7, 9),
(8, 10),
(9, 11),
(10, 12);
SET IDENTITY_INSERT [Alumnos] OFF;

SET IDENTITY_INSERT [Cursos] ON;
INSERT INTO [Cursos] (Id, Nombre, MateriaId, ProfesorId) VALUES
(1, '2024 - Programación I', 1, 1),
(2, '2024 - Base de Datos II', 2, 1),
(3, '2024 - Laboratorio III', 3, 2),
(4, '2025 - Programación I', 1, 1),
(5, '2025 - Base de Datos II', 2, 1),
(6, '2025 - Metodologías Ágiles', 4, 2);
SET IDENTITY_INSERT [Cursos] OFF;

SET IDENTITY_INSERT [Examenes] ON;
INSERT INTO [Examenes] (Id, Titulo, Inicio, Fin, PorcentajeParaAprobacion, Tipo, CursoId, Publicado) VALUES
(1, 'Parcial 1', '2024-04-15 09:00:00', '2024-04-15 11:00:00', 60.0, 0, 1, 1),
(2, 'Parcial 2', '2024-05-30 09:00:00', '2024-05-30 11:00:00', 60.0, 0, 1, 1),
(3, 'Final', '2024-07-10 14:00:00', '2024-07-10 16:00:00', 60.0, 1, 1, 1),
(4, 'Parcial 1', '2024-04-20 11:00:00', '2024-04-20 13:00:00', 60.0, 0, 2, 1),
(5, 'Final', '2024-07-12 18:00:00', '2024-07-12 20:00:00', 60.0, 1, 2, 1),
(6, 'Parcial 1: Fundamentos POO', '2025-10-20 09:00:00', '2025-10-20 11:00:00', 60.0, 0, 4, 1),
(7, 'Parcial 2: Colecciones', '2025-11-02 09:00:00', '2025-12-31 11:00:00', 60.0, 0, 4, 1),
(8, 'Final: Programación I', '2025-12-15 14:00:00', '2025-12-15 16:00:00', 60.0, 1, 4, 1),
(9, 'Parcial 1: SQL Básico (Borrador)', '2025-11-10 09:00:00', '2025-11-10 11:00:00', 60.0, 0, 5, 0);
SET IDENTITY_INSERT [Examenes] OFF;

SET IDENTITY_INSERT [Preguntas] ON;
INSERT INTO [Preguntas] (Id, Enunciado, ExamenId) VALUES
(1, '¿Qué es una variable de instancia?', 1),
(2, '¿Qué es un método estático?', 1),
(3, '¿Diferencia entre `List` y `Array`?', 2),
(4, '¿Qué hace `ArrayList`?', 2),
(5, '¿Qué es la sobrecarga de métodos (overloading)?', 3),
(6, '¿Qué es la encapsulación?', 3),
(7, '¿Qué cláusula se usa para filtrar resultados en `SELECT`?', 4),
(8, '¿Qué hace un `LEFT JOIN`?', 4),
(9, '¿Qué es una clave primaria (Primary Key)?', 5),
(10, '¿Qué es la normalización?', 5),
(11, '¿Cuál de estos pilares de la POO permite que una clase "hija" adquiera las propiedades y métodos de una clase "padre"?', 6),
(12, 'En SQL, ¿qué comando se utiliza para seleccionar y leer datos de una tabla?', 6),
(13, '¿Qué es un `String` en C#?', 7),
(14, '¿Qué hace `HashSet`?', 7),
(15, '¿Qué es el Polimorfismo?', 8),
(16, '¿Qué es una interfaz?', 8),
(17, 'Enunciado de prueba para borrador 1', 9),
(18, 'Enunciado de prueba para borrador 2', 9);
SET IDENTITY_INSERT [Preguntas] OFF;

SET IDENTITY_INSERT [Opciones] ON;
INSERT INTO [Opciones] (Id, Texto, EsCorrecta, PreguntaId) VALUES
(1, 'Una variable declarada dentro de un método', 0, 1),
(2, 'Una variable que pertenece a la clase, no al objeto', 0, 1),
(3, 'Una variable que pertenece a cada objeto (instancia)', 1, 1),
(4, 'Una variable global', 0, 1),
(5, 'Un método que no puede ser heredado', 0, 2),
(6, 'Un método que pertenece a la clase y no a la instancia', 1, 2),
(7, 'Un método que solo se usa para sumar', 0, 2),
(8, 'Un método que no devuelve valor', 0, 2),
(9, 'List tiene tamaño fijo, Array es dinámico', 0, 3),
(10, 'List es dinámico, Array tiene tamaño fijo', 1, 3),
(11, 'No hay diferencia', 0, 3),
(12, 'Ambos son de tamaño fijo', 0, 3),
(13, 'Una lista de arrays', 0, 4),
(14, 'Una versión antigua de List<T> no genérica', 1, 4),
(15, 'Un array que solo guarda números', 0, 4),
(16, 'Un tipo de base de datos', 0, 4),
(17, 'Tener varios métodos con el mismo nombre pero diferentes parámetros', 1, 5),
(18, 'Tener un método que llama a otro', 0, 5),
(19, 'Reescribir un método de la clase padre', 0, 5),
(20, 'Un método que se llama a sí mismo', 0, 5),
(21, 'Permitir acceso público a todos los atributos', 0, 6),
(22, 'Ocultar el estado interno y lógica de un objeto', 1, 6),
(23, 'Heredar de múltiples clases', 0, 6),
(24, 'Convertir un objeto a otro tipo', 0, 6),
(25, 'ORDER BY', 0, 7),
(26, 'GROUP BY', 0, 7),
(27, 'WHERE', 1, 7),
(28, 'FILTER', 0, 7),
(29, 'Combina filas de dos tablas', 0, 8),
(30, 'Devuelve todas las filas de la tabla izquierda, y las coincidentes de la derecha', 1, 8),
(31, 'Devuelve solo las filas que coinciden en ambas tablas', 0, 8),
(32, 'Devuelve todas las filas de la tabla derecha', 0, 8),
(33, 'Una clave foránea', 0, 9),
(34, 'Un identificador único para cada fila en una tabla', 1, 9),
(35, 'Un índice para búsquedas rápidas', 0, 9),
(36, 'Una restricción de tipo de dato', 0, 9),
(37, 'Proceso de optimizar consultas', 0, 10),
(38, 'Proceso de crear backups', 0, 10),
(39, 'Proceso de encriptar la base de datos', 0, 10),
(40, 'Proceso de organizar datos para reducir la redundancia', 1, 10),
(41, 'Herencia', 1, 11),
(42, 'Polimorfismo', 0, 11),
(43, 'Encapsulamiento', 0, 11),
(44, 'Abstracción', 0, 11),
(45, 'SELECT', 1, 12),
(46, 'INSERT', 0, 12),
(47, 'UPDATE', 0, 12),
(48, 'DELETE', 0, 12),
(49, 'Un tipo de valor (struct)', 0, 13),
(50, 'Un tipo de referencia inmutable', 1, 13),
(51, 'Un array de números', 0, 13),
(52, 'Una colección de chars mutable', 0, 13),
(53, 'Una colección ordenada que permite duplicados', 0, 14),
(54, 'Una colección no ordenada que no permite duplicados', 1, 14),
(55, 'Un diccionario (clave-valor)', 0, 14),
(56, 'Una cola (FIFO)', 0, 14),
(57, 'La habilidad de una variable de tomar muchas formas', 1, 15),
(58, 'Un tipo de herencia múltiple', 0, 15),
(59, 'Un bucle que cambia de forma', 0, 15),
(60, 'Una forma de guardar datos', 0, 15),
(61, 'Una clase que no puede ser instanciada', 0, 16),
(62, 'Una clase estática', 0, 16),
(63, 'Un contrato que define métodos y propiedades sin implementación', 1, 16),
(64, 'Una clase que solo tiene métodos privados', 0, 16),
(65, 'Opción A (Correcta)', 1, 17),
(66, 'Opción B', 0, 17),
(67, 'Opción C', 0, 17),
(68, 'Opción D', 0, 17),
(69, 'Opción A', 0, 18),
(70, 'Opción B (Correcta)', 1, 18),
(71, 'Opción C', 0, 18),
(72, 'Opción D', 0, 18);
SET IDENTITY_INSERT [Opciones] OFF;

SET IDENTITY_INSERT [Inscripciones] ON;
INSERT INTO [Inscripciones] (Id, Fecha, AlumnoId, CursoId) VALUES
(1, '2024-03-01', 6, 1),
(2, '2024-03-01', 7, 1),
(3, '2024-03-02', 8, 2),
(4, '2024-03-02', 9, 2),
(5, '2024-03-03', 10, 3),
(6, '2024-03-03', 6, 3),
(7, '2025-08-01', 1, 4),
(8, '2025-08-01', 2, 4),
(9, '2025-08-01', 3, 4),
(10, '2025-08-01', 4, 4),
(11, '2025-08-01', 5, 4),
(12, '2025-08-02', 1, 5),
(13, '2025-08-02', 2, 5),
(14, '2025-08-02', 3, 5),
(15, '2025-08-03', 4, 6),
(16, '2025-08-03', 5, 6),
(17, '2025-08-03', 1, 6),
(18, '2025-08-03', 2, 6);
SET IDENTITY_INSERT [Inscripciones] OFF;

SET IDENTITY_INSERT [ExamenesAlumnos] ON;
INSERT INTO [ExamenesAlumnos] (Id, Nota, CantRespCorrectas, FechaEntrega, Estado, AlumnoId, ExamenId) VALUES
(1, 100.0, 2, '2024-04-15 10:30:00', 1, 6, 1),
(2, 50.0, 1, '2024-04-15 10:45:00', 1, 7, 1),
(3, 100.0, 2, '2024-05-30 10:30:00', 1, 6, 2),
(4, 100.0, 2, '2024-05-30 10:45:00', 1, 7, 2),
(5, 100.0, 2, '2024-07-10 15:00:00', 1, 6, 3),
(6, 0.0, 0, '2024-07-10 15:10:00', 1, 7, 3),
(7, 100.0, 2, '2024-04-20 12:00:00', 1, 8, 4),
(8, 50.0, 1, '2024-04-20 12:15:00', 1, 9, 4),
(9, 50.0, 1, '2024-07-12 19:00:00', 1, 8, 5),
(10, 50.0, 1, '2024-07-12 19:15:00', 1, 9, 5),
(11, 100.0, 2, '2025-10-20 10:30:00', 1, 1, 6),
(12, 50.0, 1, '2025-10-20 10:45:00', 1, 2, 6),
(13, 0.0, 0, '2025-10-20 10:50:00', 1, 3, 6),
(14, 100.0, 2, '2025-10-20 10:55:00', 1, 4, 6),
(15, 3, 0, NULL, 3, 5, 6),
(16, 0, 0, NULL, 0, 1, 7),
(17, 0, 0, NULL, 0, 2, 7),
(18, 0, 0, NULL, 0, 1, 8),
(19, 0, 0, NULL, 0, 2, 8);
SET IDENTITY_INSERT [ExamenesAlumnos] OFF;

SET IDENTITY_INSERT [RespuestasAlumnos] ON;
INSERT INTO [RespuestasAlumnos] (Id, ExamenAlumnoId, OpcionId) VALUES
(1, 1, 3),
(2, 1, 6),
(3, 2, 2),
(4, 2, 6),
(5, 3, 10),
(6, 3, 14),
(7, 4, 10),
(8, 4, 14),
(9, 5, 17),
(10, 5, 22),
(11, 6, 18),
(12, 6, 23),
(13, 7, 27),
(14, 7, 30),
(15, 8, 27),
(16, 8, 31),
(17, 9, 34),
(18, 9, 38),
(19, 10, 36),
(20, 10, 40),
(21, 11, 41),
(22, 11, 45),
(23, 12, 41),
(24, 12, 47),
(25, 13, 42),
(26, 13, 46),
(27, 14, 41),
(28, 14, 45);
SET IDENTITY_INSERT [RespuestasAlumnos] OFF;