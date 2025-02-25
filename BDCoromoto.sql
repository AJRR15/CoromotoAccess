CREATE DATABASE BDCoromoto
GO

USE BDCoromoto

GO
CREATE TABLE [dbo].[tUsuario](
	[ConsecutivoCliente] [int] IDENTITY(1,1) NOT NULL,
	[Identificacion] [varchar](20) NOT NULL,
	[Nombre] [varchar](255) NOT NULL,
	[Apellido] [varchar](255) NOT NULL,
	[CorreoElectronico] [varchar](80) NOT NULL,
	[Telefono] [varchar](255) NOT NULL,
	[FotoPerfil] [varchar](255) NOT NULL,
	[Contrasenna] [varchar](10) NOT NULL,
	[ConsecutivoRol] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[TieneContrasennaTemp] [bit] NOT NULL ,
	[FechaVencimientoTemp] [datetime]NOT NULL ,
 CONSTRAINT [PK_tUsuario] PRIMARY KEY CLUSTERED 
(
	[ConsecutivoCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[tEmpleados](
	[ConsecutivoEmp] [int] IDENTITY(1,1) NOT NULL,
	[Identificacion] [varchar](20) NOT NULL,
	[Nombre] [varchar](255) NOT NULL,
	[Apellido] [varchar](255) NOT NULL,
	[CorreoElectronico] [varchar](80) NOT NULL,
	[Telefono] [varchar](255) NOT NULL,
	[FotoPerfil] [varchar](255) NOT NULL,
	[Contrasenna] [varchar](10) NOT NULL,
	[ConsecutivoRol] [int] NOT NULL,
	[Activo] [bit] NOT NULL,
	[TieneContrasennaTemp] [bit] NOT NULL,
	[FechaVencimientoTemp] [datetime] NOT NULL,
	[IdVilla] [int] NOT NULL,                          
	[IdTurno] [int] NOT NULL,						   
	[Vacaciones] [int] NOT NULL,
	[HorasTrabajadas] [int] NOT NULL 
 CONSTRAINT [PK_tEmpleados] PRIMARY KEY CLUSTERED 
(
	[ConsecutivoEmp] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[tRoles](
	[IdRol] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](50) NOT NULL
 CONSTRAINT [PK_tRoles] PRIMARY KEY CLUSTERED 
(
	[IdRol] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tVillas](
	[IdVilla] [int] IDENTITY(1,1) NOT NULL,
	[NombreHabitacion] [varchar](255) NOT NULL
 CONSTRAINT [PK_tVillas] PRIMARY KEY CLUSTERED 
(
	[IdVilla] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tHabitaciones](
	[IdHabitacion] [int] IDENTITY(1,1) NOT NULL,
	[NombreHabitacion] [varchar](255) NOT NULL,
	[Descripcion] [varchar](255) NOT NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[CheckIn] [datetime] NOT NULL,
	[CheckOut] [datetime] NOT NULL,
	[Estado] [bit] NOT NULL,
	[IdVilla] [int] not null,
	[IdTipoHabitacion] [int] not null
 CONSTRAINT [PK_tHabitaciones] PRIMARY KEY CLUSTERED 
(
	[IdHabitacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tReservas](
	[IdReserva] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[IdHabitacion] [int] NOT NULL,
	[CheckIn] [datetime] NOT NULL,
	[CheckOut] [datetime] NOT NULL,
	[Estado] [bit] NOT NULL,
	[IdMoneda] [int] NOT NULL,
	[IdMetodoP] [int] NOT NULL
 CONSTRAINT [PK_tReservas] PRIMARY KEY CLUSTERED 
(
	[IdReserva] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tLimpiezas](
	[IdLimpieza] [int] IDENTITY(1,1) NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[IdHabitacion] [int] NOT NULL,
	[FechaLimpieza] [datetime] NOT NULL,
	[Estado] [bit] NOT NULL
 CONSTRAINT [PK_tLimpiezas] PRIMARY KEY CLUSTERED 
(
	[IdLimpieza] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tActivos](
	[IdActivo] [int] IDENTITY(1,1) NOT NULL,
	[IdHabitacion] [int] NOT NULL,
	[Nombre] [varchar](255) NOT NULL,
	[Modelo] [varchar](255) NOT NULL,
	[NumeroSerie] [varchar](255) NOT NULL,
	[Descripcion] [varchar](255) NOT NULL,
	[IdCategoria] [int] NOT NULL
 CONSTRAINT [PK_tActivos] PRIMARY KEY CLUSTERED 
(
	[IdActivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tCategorias](
	[IdCategoria] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](255) NOT NULL
 CONSTRAINT [PK_tCategorias] PRIMARY KEY CLUSTERED 
(
	[IdCategoria] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tContactenos](
	[IdContacto] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[Mensaje] [varchar](255) NOT NULL
 CONSTRAINT [PK_tContactenos] PRIMARY KEY CLUSTERED 
(
	[IdContacto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tTurnos](
	[IdTurno] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](255) NOT NULL,
	[HoraInicio] [varchar](255) NOT NULL,
	[HoraSalida] [varchar](255) NOT NULL
 CONSTRAINT [PK_tTurnos] PRIMARY KEY CLUSTERED 
(
	[IdTurno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tVacaciones](
	[IdVacacion] [int] IDENTITY(1,1) NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[DiasSolicitados] [varchar](255) NOT NULL,
	[fechaInicio] [dateTime] NOT NULL,
	[fechaFin] [dateTime] NOT NULL,
	[Estado] [int]
 CONSTRAINT [PK_tVacaciones] PRIMARY KEY CLUSTERED 
(
	[IdVacacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tNominas](
	[IdNomina] [int] IDENTITY(1,1) NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[Salario] [decimal] NOT NULL,
	[Bono] [decimal] NOT NULL,
	[Multa] [decimal] NOT NULL,
	[Total] [decimal] NOT NULL
 CONSTRAINT [PK_tNominas] PRIMARY KEY CLUSTERED 
(
	[IdNomina] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tEvaluaciones](
	[IdEvaluacion] [int] IDENTITY(1,1) NOT NULL,
	[IdEmpleado] [int] NOT NULL,
	[Comentario] [varchar](255) NOT NULL,
	[Calificacion] [int] NOT NULL,
	[FechaEvaluacion] [dateTime]
 CONSTRAINT [PK_tEvaluaciones] PRIMARY KEY CLUSTERED 
(
	[IdEvaluacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tFacturas](
	[IdFactura] [int] IDENTITY(1,1) NOT NULL,
	[IdUsuario] [int] NOT NULL,
	[FechaEmision] [dateTime] NOT NULL,
	[Imagen] [varchar](255) NOT NULL,
	[Total] [varchar](255) NOT NULL,
	[Estado] [bit] NOT NULL
 CONSTRAINT [PK_tFacturas] PRIMARY KEY CLUSTERED 
(
	[IdFactura] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

--esta tabla permisos esta sujeta a cambios ya que no se ha llegado a un idea clara de como manejarse--
CREATE TABLE [dbo].[tPermisos](
	[IdPermiso] [int] IDENTITY(1,1) NOT NULL,
	[IdRol] [int] NOT NULL,
	[NombrePermiso] [varchar] (255) NOT NULL,
	[Estado] [bit] NOT NULL
 CONSTRAINT [PK_tPermisos] PRIMARY KEY CLUSTERED 
(
	[IdPermiso] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


CREATE TABLE [dbo].[tMetodoPago](
	[IdMetodoP] [int] IDENTITY(1,1) NOT NULL,
	[NombreMetodoP] [varchar] (255) NOT NULL,
 CONSTRAINT [PK_tMetodoPago] PRIMARY KEY CLUSTERED 
(
	[IdMetodoP] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tTiposHabitaciones](
	[IdTipodeHabitacion] [int] IDENTITY(1,1) NOT NULL,
	[NombreTipodeHabitcion] [varchar] (255) NOT NULL,
 CONSTRAINT [PK_tTiposHabitaciones] PRIMARY KEY CLUSTERED 
(
	[IdTipodeHabitacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tMonedas](
	[IdMoneda] [int] IDENTITY(1,1) NOT NULL,
	[NombreMoneda] [varchar] (255) NOT NULL,
 CONSTRAINT [PK_tMonedas] PRIMARY KEY CLUSTERED 
(
	[IdMoneda] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Foreign Keys --

ALTER TABLE [dbo].[tUsuario]
ADD CONSTRAINT FK_tUsuario_tRoles
FOREIGN KEY ([ConsecutivoRol])
REFERENCES [dbo].[tRoles]([IdRol]);

ALTER TABLE [dbo].[tEmpleados]
ADD CONSTRAINT FK_tEmpleados_tRoles
FOREIGN KEY ([ConsecutivoRol])
REFERENCES [dbo].[tRoles]([IdRol]),
CONSTRAINT FK_tEmpleados_tVillas
FOREIGN KEY ([IdVilla])
REFERENCES [dbo].[tVillas]([IdVilla]),
CONSTRAINT FK_tEmpleados_tTurnos
FOREIGN KEY ([IdTurno])
REFERENCES [dbo].[tTurnos]([IdTurno]);

ALTER TABLE [dbo].[tHabitaciones]
ADD CONSTRAINT FK_tHabitaciones_tVillas
FOREIGN KEY ([IdVilla])
REFERENCES [dbo].[tVillas]([IdVilla]),
CONSTRAINT FK_tHabitaciones_tTipoHabitacion
FOREIGN KEY ([IdTipoHabitacion])
REFERENCES [dbo].[tTiposHabitaciones]([IdTipodeHabitacion]);

ALTER TABLE [dbo].[tReservas]
ADD CONSTRAINT FK_tReservas_tUsuario
FOREIGN KEY ([IdUsuario])
REFERENCES [dbo].[tUsuario]([ConsecutivoCliente]),
CONSTRAINT FK_tReservas_tHabitaciones
FOREIGN KEY ([IdHabitacion])
REFERENCES [dbo].[tHabitaciones]([IdHabitacion]),
CONSTRAINT FK_tReservas_tMonedas
FOREIGN KEY ([IdMoneda])
REFERENCES [dbo].[tMonedas]([IdMoneda]),
CONSTRAINT FK_tReservas_tMetodoPago
FOREIGN KEY ([IdMetodoP])
REFERENCES [dbo].[tMetodoPago]([IdMetodoP]);

ALTER TABLE [dbo].[tLimpiezas]
ADD CONSTRAINT FK_tLimpiezas_tEmpleados
FOREIGN KEY ([IdEmpleado])
REFERENCES [dbo].[tEmpleados]([ConsecutivoEmp]),
CONSTRAINT FK_tLimpiezas_tHabitaciones
FOREIGN KEY ([IdHabitacion])
REFERENCES [dbo].[tHabitaciones]([IdHabitacion]);

ALTER TABLE [dbo].[tActivos]
ADD CONSTRAINT FK_tActivos_tHabitaciones
FOREIGN KEY ([IdHabitacion])
REFERENCES [dbo].[tHabitaciones]([IdHabitacion]),
CONSTRAINT FK_tActivos_tCategorias
FOREIGN KEY ([IdCategoria])
REFERENCES [dbo].[tCategorias]([IdCategoria]);

ALTER TABLE [dbo].[tContactenos]
ADD CONSTRAINT FK_tContactenos_tUsuario
FOREIGN KEY ([IdUsuario])
REFERENCES [dbo].[tUsuario]([ConsecutivoCliente]);

ALTER TABLE [dbo].[tVacaciones]
ADD CONSTRAINT FK_tVacaciones_tEmpleados
FOREIGN KEY ([IdEmpleado])
REFERENCES [dbo].[tEmpleados]([ConsecutivoEmp]);

ALTER TABLE [dbo].[tNominas]
ADD CONSTRAINT FK_tNominas_tEmpleados
FOREIGN KEY ([IdEmpleado])
REFERENCES [dbo].[tEmpleados]([ConsecutivoEmp]);

ALTER TABLE [dbo].[tEvaluaciones]
ADD CONSTRAINT FK_tEvaluaciones_tEmpleados
FOREIGN KEY ([IdEmpleado])
REFERENCES [dbo].[tEmpleados]([ConsecutivoEmp]);

ALTER TABLE [dbo].[tFacturas]
ADD CONSTRAINT FK_tFacturas_tUsuario
FOREIGN KEY ([IdUsuario])
REFERENCES [dbo].[tUsuario]([ConsecutivoCliente]);

ALTER TABLE [dbo].[tPermisos]
ADD CONSTRAINT FK_tPermisos_tRoles
FOREIGN KEY ([IdRol])
REFERENCES [dbo].[tRoles]([IdRol]);

INSERT INTO [BDCoromoto].[dbo].[tRoles] ([NombreRol])
VALUES ('Administrador');

INSERT INTO [BDCoromoto].[dbo].[tRoles] ([NombreRol])
VALUES ('Empleado');

INSERT INTO [BDCoromoto].[dbo].[tRoles] ( [NombreRol])
VALUES ('Cliente');

GO

CREATE PROCEDURE [dbo].[RegistroUsuario]
	@Identificacion		varchar(20),
	@Nombre				varchar(255),
	@Apellido			varchar(255),
	@CorreoElectronico	varchar(80),
	@Telefono				varchar(255),
	@FotoPerfil         varchar(255),
	@Contrasenna		varchar(10)
AS
BEGIN

	IF NOT EXISTS(	SELECT	1 
					FROM	dbo.tUsuario 
					WHERE	Identificacion = @Identificacion
						AND	CorreoElectronico = @CorreoElectronico)
	BEGIN
		INSERT INTO dbo.tUsuario (Identificacion,Nombre,Apellido,CorreoElectronico,Telefono,FotoPerfil,Contrasenna, ConsecutivoRol,Activo,TieneContrasennaTemp,FechaVencimientoTemp)
		VALUES (@Identificacion,@Nombre,@Apellido,@CorreoElectronico,@Telefono,@FotoPerfil,@Contrasenna,3,1,0,GETDATE())
	END

END

GO
/****** Object:  StoredProcedure [dbo].[InicioSesionEmpleado]    Script Date: 2/2/2025 12:53:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[InicioSesionEmpleado]
	@Identificacion		varchar(20),
	@Contrasenna		varchar(10)
AS
BEGIN

	SELECT	U.ConsecutivoEmp,
			Identificacion,
			Nombre,
			Apellido,
			CorreoElectronico,
			Telefono,
			FotoPerfil,
			Contrasenna,
			ConsecutivoRol,
			Activo,
			R.NombreRol,
			TieneContrasennaTemp,
			FechaVencimientoTemp,
			IdVilla,
			IdTurno,
			Vacaciones,
			HorasTrabajadas
	  FROM	dbo.tEmpleados U 
	  INNER JOIN dbo.tRoles R ON U.ConsecutivoRol = R.IdRol
	  WHERE Identificacion = @Identificacion
		AND	Contrasenna	= @Contrasenna
		AND Activo = 1

END

GO
/****** Object:  StoredProcedure [dbo].[InicioSesionUsuario]    Script Date: 2/2/2025 12:54:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[InicioSesionUsuario]
	@Identificacion		varchar(20),
	@Contrasenna		varchar(10)
AS
BEGIN

	SELECT	U.ConsecutivoCliente,
			Identificacion,
			Nombre,
			Apellido,
			CorreoElectronico,
			Telefono,
			FotoPerfil,
			Contrasenna,
			ConsecutivoRol,
			Activo,
			R.NombreRol,
			TieneContrasennaTemp,
			FechaVencimientoTemp
	  FROM	dbo.tUsuario U 
	  INNER JOIN dbo.tRoles R ON U.ConsecutivoRol = R.IdRol
	  WHERE Identificacion = @Identificacion
		AND	Contrasenna	= @Contrasenna
		AND Activo = 1

END
--------------------------------------------------------------------------------------------------------------------------
CREATE TABLE [dbo].[tTareas](
    [IdTarea] [int] IDENTITY(1,1) NOT NULL,
    [Descripcion] [varchar](255) NOT NULL,
    [Estado] [bit] NOT NULL,
    [IdEmpleado] [int] NOT NULL,
 CONSTRAINT [PK_tTareas] PRIMARY KEY CLUSTERED 
(
    [IdTarea] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tTareas]
ADD CONSTRAINT FK_tTareas_tEmpleados
FOREIGN KEY ([IdEmpleado])
REFERENCES [dbo].[tEmpleados]([ConsecutivoEmp]);
GO


--Agregar el insert de las categoria.-- 
INSERT INTO [BDCoromoto].[dbo].[tCategorias] ( Nombre)
VALUES ('Categor�a 1');
