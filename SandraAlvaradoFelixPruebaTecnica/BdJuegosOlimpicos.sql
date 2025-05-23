USE [BdJuegosOlimpicos]
GO
/****** Object:  Table [dbo].[tbAuditoria]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbAuditoria](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[evento] [nvarchar](max) NULL,
	[evento_descripcion] [nvarchar](max) NULL,
	[usuario] [int] NULL,
	[cliente_ip] [nvarchar](100) NULL,
	[nombre_procedimiento] [nvarchar](max) NULL,
	[json_in_nv_request] [nvarchar](max) NULL,
	[json_out_nv_response] [nvarchar](max) NULL,
	[json_out_nv] [nvarchar](max) NULL,
	[fecha_creacion] [datetime] NULL,
	[host_creacion] [varchar](50) NULL,
 CONSTRAINT [PK_tbAuditoria] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbAuditoriaLevantamiento]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbAuditoriaLevantamiento](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[json_in_nv_request] [varchar](max) NULL,
	[fecha_creacion] [datetime] NULL,
	[host_creacion] [varchar](50) NULL,
 CONSTRAINT [PK_tb_auditoria_levantamiento] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbDeportistas]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbDeportistas](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](100) NULL,
	[pais_id] [int] NULL,
	[fecha_creacion] [datetime] NULL,
	[host_creacion] [varchar](50) NULL,
 CONSTRAINT [PK_tbDeportistas] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbLevantamientos]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbLevantamientos](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[deportista_id] [int] NULL,
	[modalidad] [varchar](50) NULL,
	[peso] [int] NULL,
	[fecha_intento] [datetime] NULL,
	[fecha_creacion] [datetime] NULL,
	[host_creacion] [varchar](50) NULL,
 CONSTRAINT [PK_tbLevantamientos] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbPaises]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbPaises](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[codigo_pais] [varchar](3) NULL,
	[nombre] [varchar](200) NULL,
	[fecha_creacion] [datetime] NULL,
	[host_creacion] [varchar](50) NULL,
 CONSTRAINT [PK_tbPais] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tbUsuarios]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tbUsuarios](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nombre] [varchar](250) NULL,
	[usuario] [varchar](50) NULL,
	[contrasena] [varchar](200) NULL,
	[rol] [varchar](50) NULL,
	[correo] [varchar](250) NULL,
	[fecha_creacion] [datetime] NULL,
	[host_creacion] [varchar](50) NULL,
	[estado] [int] NULL,
 CONSTRAINT [PK_tbUsuario] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tbAuditoria] ADD  CONSTRAINT [DF_tbAuditoria_fecha_creacion]  DEFAULT (getdate()) FOR [fecha_creacion]
GO
ALTER TABLE [dbo].[tbAuditoria] ADD  CONSTRAINT [DF_tbAuditoria_host_creacion]  DEFAULT (host_name()) FOR [host_creacion]
GO
ALTER TABLE [dbo].[tbAuditoriaLevantamiento] ADD  CONSTRAINT [DF_tb_auditoria_levantamiento_fecha_creacion]  DEFAULT (getdate()) FOR [fecha_creacion]
GO
ALTER TABLE [dbo].[tbAuditoriaLevantamiento] ADD  CONSTRAINT [DF_tb_auditoria_levantamiento_host_creacion]  DEFAULT (host_name()) FOR [host_creacion]
GO
ALTER TABLE [dbo].[tbDeportistas] ADD  CONSTRAINT [DF_tbDeportistas_fecha_creacion]  DEFAULT (getdate()) FOR [fecha_creacion]
GO
ALTER TABLE [dbo].[tbDeportistas] ADD  CONSTRAINT [DF_tbDeportistas_host_creacion]  DEFAULT (host_name()) FOR [host_creacion]
GO
ALTER TABLE [dbo].[tbLevantamientos] ADD  CONSTRAINT [DF_tbLevantamientos_fecha_intento]  DEFAULT (getdate()) FOR [fecha_intento]
GO
ALTER TABLE [dbo].[tbLevantamientos] ADD  CONSTRAINT [DF_tbLevantamientos_fecha_creacion]  DEFAULT (getdate()) FOR [fecha_creacion]
GO
ALTER TABLE [dbo].[tbLevantamientos] ADD  CONSTRAINT [DF_tbLevantamientos_host_creacion]  DEFAULT (host_name()) FOR [host_creacion]
GO
ALTER TABLE [dbo].[tbPaises] ADD  CONSTRAINT [DF_tbPais_fecha_creacion]  DEFAULT (getdate()) FOR [fecha_creacion]
GO
ALTER TABLE [dbo].[tbPaises] ADD  CONSTRAINT [DF_tbPais_host_creacion]  DEFAULT (host_name()) FOR [host_creacion]
GO
ALTER TABLE [dbo].[tbUsuarios] ADD  CONSTRAINT [DF_tbUsuario_fecha_creacion]  DEFAULT (getdate()) FOR [fecha_creacion]
GO
ALTER TABLE [dbo].[tbUsuarios] ADD  CONSTRAINT [DF_tbUsuario_host_creacion]  DEFAULT (host_name()) FOR [host_creacion]
GO
ALTER TABLE [dbo].[tbUsuarios] ADD  CONSTRAINT [DF_tbUsuario_estado]  DEFAULT ((1)) FOR [estado]
GO
ALTER TABLE [dbo].[tbDeportistas]  WITH CHECK ADD  CONSTRAINT [FK_tbDeportistas_tbPaises] FOREIGN KEY([pais_id])
REFERENCES [dbo].[tbPaises] ([id])
GO
ALTER TABLE [dbo].[tbDeportistas] CHECK CONSTRAINT [FK_tbDeportistas_tbPaises]
GO
ALTER TABLE [dbo].[tbLevantamientos]  WITH CHECK ADD  CONSTRAINT [FK_tbLevantamientos_tbDeportistas] FOREIGN KEY([deportista_id])
REFERENCES [dbo].[tbDeportistas] ([id])
GO
ALTER TABLE [dbo].[tbLevantamientos] CHECK CONSTRAINT [FK_tbLevantamientos_tbDeportistas]
GO
/****** Object:  StoredProcedure [dbo].[usp_auditoria_c]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_auditoria_c]
	@origen						nvarchar(100)= NULL,
	@evento						nvarchar(100)= NULL,
	@evento_descripcion			nvarchar(100)= NULL,
	@usuario					INT			 = NULL,
	@cliente_ip					nvarchar(100)= NULL,
	@nombre_procedimiento		nvarchar(100)= NULL,
	@json_in_nv_request			nvarchar(max)= NULL,
	@json_out_nv_response		nvarchar(max)= NULL,
	@json_out_nv				nvarchar(MAX)= NULL
AS
BEGIN
	DECLARE @error_i int
	DECLARE @error_nv nvarchar(MAX)
	DECLARE @procedure_name_nv nvarchar(MAX)
	DECLARE @msg_out nvarchar(max)
	DECLARE @result int
	DECLARE @json_result_nv nvarchar(MAX);
	
	SET @procedure_name_nv =(SELECT OBJECT_NAME(@@PROCID))

	DECLARE @json_in_schema_nv nvarchar(MAX)=N'{"evento":"text","evento_descripcion":"text","usuario":"int","client_ip":"text","nombre_procedimiento":"text","json_in_nv_request":"text","json_out_nv_response":"text"}';

	DECLARE @tran nvarchar(100)='usp_tbsma_auditory_c'
	BEGIN TRANSACTION @tran
	BEGIN TRY	
		
		INSERT INTO tbAuditoria(origen,evento,evento_descripcion,usuario,cliente_ip,nombre_procedimiento,json_in_nv_request,json_out_nv_response)
		VALUES(@origen,@evento,@evento_descripcion,@usuario,@cliente_ip,@nombre_procedimiento,@json_in_nv_request,@json_out_nv_response)
		
		
	COMMIT TRANSACTION @tran

	END TRY
	BEGIN CATCH
		
		print error_message()

		ROLLBACK TRANSACTION @tran
		
		
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[usp_auditoria_intentos_levantamiento_c]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_auditoria_intentos_levantamiento_c]
	@json_in_nv_request			nvarchar(max)= NULL
AS
BEGIN
	
	DECLARE @tran nvarchar(100)='usp_tbsma_auditory_c'
	BEGIN TRANSACTION @tran
	BEGIN TRY	
		
		INSERT INTO tbAuditoriaLevantamiento(json_in_nv_request)
		VALUES(@json_in_nv_request)
		
		
	COMMIT TRANSACTION @tran

	END TRY
	BEGIN CATCH
		
		print error_message()

		ROLLBACK TRANSACTION @tran
		
		
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[usp_deportista_c]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_deportista_c]
	@json_in_nv nvarchar(MAX),
	@json_out_nv nvarchar(MAX) out
AS
BEGIN
	DECLARE @error_i int, @error_nv nvarchar(MAX), @procedure_name_nv nvarchar(MAX),@msg_out nvarchar(max), @result int, @json_result_nv nvarchar(MAX);
	DECLARE @event varchar(200)='Crear deportista';
	DECLARE @evento_descripcion varchar(200)='Regista información de un deportista ';

	SET @procedure_name_nv =(SELECT OBJECT_NAME(@@PROCID))
	DECLARE @json_in_schema_nv nvarchar(MAX)=N'{"user_id_i":"int","nombre":"text","pais_id":"int","ip_client":"text"}';
	
	DECLARE @tran nvarchar(100)='usp_deportista_c'
	BEGIN TRANSACTION @tran
	BEGIN TRY			

		IF(COALESCE(ISJSON(@json_in_nv),0)=0)
		BEGIN
			SET @error_i = 50100
			SET @error_nv = CONCAT('{"procedure_name_nv":"',@procedure_name_nv,'","error_nv":"El json no es valido.","error_id_i":"-',@error_i,'","json_in_nv_proc":',@json_in_nv,'}');
			THROW @error_i,@error_nv,1;
		END
		ELSE
		BEGIN 
			DECLARE @ip_client varchar(50)=isnull(CAST(JSON_VALUE(@json_in_nv, '$.ip_client')as varchar(50)),'0');			
			DECLARE	@user_id_i INT=CAST(JSON_VALUE(@json_in_nv, '$.user_id_i')as int);
			DECLARE	@nombre VARCHAR(100)=CAST(JSON_VALUE(@json_in_nv, '$.nombre')as VARCHAR(100));	
			DECLARE	@pais_id INT=CAST(JSON_VALUE(@json_in_nv, '$.pais_id')as int);
			DECLARE @id int;

			BEGIN 				
				INSERT INTO tbDeportistas(nombre,pais_id)
				VALUES (@nombre,@pais_id)

				SET @id=SCOPE_IDENTITY()

				SET @json_result_nv=(SELECT @id code FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)				
				SET @result=1	
																	
			END
			
			--AQUI CONCLUYE LA FUNCIONALIDAD -------------------------------------------------------------------------------------------------------------------------------------------------------------------									
			SET @json_out_nv=(SELECT @result result, JSON_QUERY(@json_result_nv) as json_result_nv, JSON_QUERY(@json_in_schema_nv) json_in_schema_nv, NULL as error_nv FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)
			
		END

		COMMIT TRANSACTION @tran

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @tran
		print error_message()
		IF(@procedure_name_nv IS NULL)
		BEGIN
			SET @error_i = 50001
			SET @error_nv = CONCAT('{"error_id_i":"-',@error_i,'","procedure_name_nv":"',@procedure_name_nv,'","error_nv":"',ERROR_MESSAGE(),'"}');
			THROW @error_i,@error_nv,1;
		END 
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[usp_intentos_levantamientos_l]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_intentos_levantamientos_l]
	@json_in_nv nvarchar(MAX),
	@json_out_nv nvarchar(MAX) out
AS
BEGIN
	DECLARE @error_i int, @error_nv nvarchar(MAX), @procedure_name_nv nvarchar(MAX),@msg_out nvarchar(max), @result int, @json_result_nv nvarchar(MAX);

	SET @procedure_name_nv =(SELECT OBJECT_NAME(@@PROCID))
	DECLARE @json_in_schema_nv nvarchar(MAX)=N'{"user_id_i":"int","skip_i":"int","take_i":"int","ip_client":"text"}';
	
	DECLARE @tran nvarchar(100)='usp_intentos_levantamientos_l'
	BEGIN TRANSACTION @tran
	BEGIN TRY			

		IF(COALESCE(ISJSON(@json_in_nv),0)=0)
		BEGIN
			SET @error_i = 50100
			SET @error_nv = CONCAT('{"procedure_name_nv":"',@procedure_name_nv,'","error_nv":"El json no es valido.","error_id_i":"-',@error_i,'","json_in_nv_proc":',@json_in_nv,'}');
			THROW @error_i,@error_nv,1;
		END
		ELSE
		BEGIN 
			DECLARE @ip_client varchar(50)=isnull(CAST(JSON_VALUE(@json_in_nv, '$.ip_client')as varchar(50)),'0');			
			DECLARE	@user_id_i INT=CAST(JSON_VALUE(@json_in_nv, '$.user_id_i')as int);
			DECLARE @skip_i int=ISNULL(CAST(JSON_VALUE(@json_in_nv, '$.skip_i')as int),0);
			DECLARE @take_i int=ISNULL(CAST(JSON_VALUE(@json_in_nv, '$.take_i')as int),99999999);


			BEGIN 
				
				SET @json_result_nv=
				(
					SELECT 
						d.nombre, 
						JSON_VALUE(a.json_in_nv_request, '$.modalidad') AS modalidad,
						COUNT(*) AS total_intentos
					FROM 
						tbAuditoriaLevantamiento a
					JOIN 
						tbDeportistas d 
						ON d.id = JSON_VALUE(a.json_in_nv_request, '$.deportista_id')
					GROUP BY 
						d.nombre,JSON_VALUE(json_in_nv_request, '$.deportista_id'),JSON_VALUE(a.json_in_nv_request, '$.modalidad')
					ORDER BY 
						total_intentos DESC
					OFFSET @skip_i ROWS
					FETCH NEXT @take_i ROWS ONLY FOR JSON PATH, INCLUDE_NULL_VALUES)
				
				SET @result=1	
																	
			END
			
			--AQUI CONCLUYE LA FUNCIONALIDAD -------------------------------------------------------------------------------------------------------------------------------------------------------------------									
			SET @json_out_nv=(SELECT @result result, JSON_QUERY(@json_result_nv) as json_result_nv, JSON_QUERY(@json_in_schema_nv) json_in_schema_nv, NULL as error_nv FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)
			
		END

		COMMIT TRANSACTION @tran

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @tran
		print error_message()
		IF(@procedure_name_nv IS NULL)
		BEGIN
			SET @error_i = 50001
			SET @error_nv = CONCAT('{"error_id_i":"-',@error_i,'","procedure_name_nv":"',@procedure_name_nv,'","error_nv":"',ERROR_MESSAGE(),'"}');
			THROW @error_i,@error_nv,1;
		END 
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[usp_levantamiento_c]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_levantamiento_c]
	@json_in_nv nvarchar(MAX),
	@json_out_nv nvarchar(MAX) out
AS
BEGIN
	DECLARE @error_i int, @error_nv nvarchar(MAX), @procedure_name_nv nvarchar(MAX),@msg_out nvarchar(max), @result int, @json_result_nv nvarchar(MAX);
	DECLARE @event varchar(200)='Crear levantamientos';
	DECLARE @evento_descripcion varchar(200)='Regista información de un levantamientos por persona ';

	SET @procedure_name_nv =(SELECT OBJECT_NAME(@@PROCID))
	DECLARE @json_in_schema_nv nvarchar(MAX)=N'{"user_id_i":"int","deportista_id":"int","modalidad":"text","peso":"int","ip_client":"text"}';
	DECLARE @tran nvarchar(100)='usp_levantamiento_c'

	INSERT INTO tbAuditoriaLevantamiento(json_in_nv_request)
	VALUES(@json_in_nv)

	BEGIN TRANSACTION @tran
	BEGIN TRY			

		IF(COALESCE(ISJSON(@json_in_nv),0)=0)
		BEGIN
			SET @error_i = 50100
			SET @error_nv = CONCAT('{"procedure_name_nv":"',@procedure_name_nv,'","error_nv":"El json no es valido.","error_id_i":"-',@error_i,'","json_in_nv_proc":',@json_in_nv,'}');
			THROW @error_i,@error_nv,1;
		END
		ELSE
		BEGIN 
			DECLARE @ip_client varchar(50)=isnull(CAST(JSON_VALUE(@json_in_nv, '$.ip_client')as varchar(50)),'0');			
			DECLARE	@user_id_i INT=CAST(JSON_VALUE(@json_in_nv, '$.user_id_i')as int);
			DECLARE	@deportista_id INT=CAST(JSON_VALUE(@json_in_nv, '$.deportista_id')as int);
			DECLARE	@modalidad VARCHAR(100)=CAST(JSON_VALUE(@json_in_nv, '$.modalidad')as VARCHAR(100));	
			DECLARE	@peso INT=CAST(JSON_VALUE(@json_in_nv, '$.peso')as int);
			DECLARE @id int;
			DECLARE @registros_count INT;

			SELECT @registros_count = COUNT(*)
			FROM tblevantamientos
			WHERE deportista_id = @deportista_id AND modalidad = @modalidad;

			IF @registros_count >= 3
			BEGIN
				print 'error'
				SET @error_i = 50401
				SET @json_out_nv = CONCAT('{"procedure_name_nv":"', @procedure_name_nv, '","error_nv":"El deportista ya tiene 3 intentos en esta modalidad.","error_id_i":"-', @error_i, '","json_in_nv_proc":', @json_in_nv, '}');
				SET @result=0;
				THROW @error_i,@error_nv,1

			END


			INSERT INTO tbLevantamientos(deportista_id,modalidad,peso)
			VALUES (@deportista_id,@modalidad,@peso)

			SET @id=SCOPE_IDENTITY()

			SET @json_result_nv=(SELECT @id code FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)				
			SET @result=1	
																	
			
			--AQUI CONCLUYE LA FUNCIONALIDAD -------------------------------------------------------------------------------------------------------------------------------------------------------------------									
			SET @json_out_nv=(SELECT @result result, JSON_QUERY(@json_result_nv) as json_result_nv, JSON_QUERY(@json_in_schema_nv) json_in_schema_nv, NULL as error_nv FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)
			
		END
		set @error_i=0
		set @error_nv=''
		COMMIT TRANSACTION @tran

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @tran
		print error_message()
		IF(@procedure_name_nv IS NULL)
		BEGIN
			SET @error_i = 50001
			SET @error_nv = CONCAT('{"error_id_i":"-',@error_i,'","procedure_name_nv":"',@procedure_name_nv,'","error_nv":"',ERROR_MESSAGE(),'"}');
			set @result=0;
			THROW @error_i,@error_nv,1;

		END 
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[usp_resultados_l]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_resultados_l]
	@json_in_nv nvarchar(MAX),
	@json_out_nv nvarchar(MAX) out
AS
BEGIN
	DECLARE @error_i int, @error_nv nvarchar(MAX), @procedure_name_nv nvarchar(MAX),@msg_out nvarchar(max), @result int, @json_result_nv nvarchar(MAX);
	DECLARE @event varchar(200)='Listar resultados';
	DECLARE @evento_descripcion varchar(200)='Lista los resultados de las personas registradas para el levantamiento de halterofilia';

	SET @procedure_name_nv =(SELECT OBJECT_NAME(@@PROCID))
	DECLARE @json_in_schema_nv nvarchar(MAX)=N'{"user_id_i":"int","skip_i":"int","take_i":"int","ip_client":"text"}';
	
	DECLARE @tran nvarchar(100)='usp_resultados_l'
	BEGIN TRANSACTION @tran
	BEGIN TRY			

		IF(COALESCE(ISJSON(@json_in_nv),0)=0)
		BEGIN
			SET @error_i = 50100
			SET @error_nv = CONCAT('{"procedure_name_nv":"',@procedure_name_nv,'","error_nv":"El json no es valido.","error_id_i":"-',@error_i,'","json_in_nv_proc":',@json_in_nv,'}');
			THROW @error_i,@error_nv,1;
		END
		ELSE
		BEGIN 
			DECLARE @ip_client varchar(50)=isnull(CAST(JSON_VALUE(@json_in_nv, '$.ip_client')as varchar(50)),'0');			
			DECLARE	@user_id_i INT=CAST(JSON_VALUE(@json_in_nv, '$.user_id_i')as int);
			DECLARE @skip_i int=ISNULL(CAST(JSON_VALUE(@json_in_nv, '$.skip_i')as int),0);
			DECLARE @take_i int=ISNULL(CAST(JSON_VALUE(@json_in_nv, '$.take_i')as int),99999999);


			BEGIN 
				DECLARE @json_list_nv nvarchar(MAX)
				DECLARE @total_i INT;

				SELECT 
					p.codigo_pais AS pais,
					d.nombre AS nombre,
					ISNULL(MAX(CASE WHEN l.modalidad = 'arranque' THEN l.peso END), 0) AS aranque,
					ISNULL(MAX(CASE WHEN l.modalidad = 'envion' THEN l.peso END), 0) AS envion,
					ISNULL(MAX(CASE WHEN l.modalidad = 'arranque' THEN l.peso END), 0) +
					ISNULL(MAX(CASE WHEN l.modalidad = 'envion' THEN l.peso END), 0) AS totalpeso,
					ROW_NUMBER() OVER (ORDER BY 
					ISNULL(MAX(CASE WHEN l.modalidad = 'arranque' THEN l.peso END), 0) +
					ISNULL(MAX(CASE WHEN l.modalidad = 'envion' THEN l.peso END), 0) DESC) AS ord_i
				INTO #tmpResults  
				FROM tbDeportistas d
				INNER JOIN tbPaises p ON d.pais_id = p.id
				INNER JOIN tblevantamientos l ON l.deportista_id = d.id
				GROUP BY d.id, d.nombre, p.codigo_pais
				ORDER BY totalpeso DESC;
				--OFFSET @skip_i ROWS
				--FETCH NEXT @take_i ROWS ONLY;

				SELECT @total_i = COUNT(*) FROM #tmpResults;

				SET @json_result_nv=
				(
					SELECT 
						pais			AS	Pais,
						nombre			AS  Nombre,
						aranque			AS  Arranque,
						envion			AS  Envion,
						totalpeso		AS  TotalPeso
					FROM #tmpResults
					WHERE ord_i > COALESCE(@skip_i, 0) 
					AND ord_i <= (COALESCE(@skip_i, 0) + COALESCE(@take_i, 0))

					FOR JSON PATH, INCLUDE_NULL_VALUES)

				
				SET @result=1	
	
																	
			END
			
			--AQUI CONCLUYE LA FUNCIONALIDAD -------------------------------------------------------------------------------------------------------------------------------------------------------------------									
			SET @json_out_nv=(SELECT @result result, JSON_QUERY(@json_result_nv) as json_result_nv, JSON_QUERY(@json_in_schema_nv) json_in_schema_nv, NULL as error_nv FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)
			
		END

		COMMIT TRANSACTION @tran

	END TRY
	BEGIN CATCH
		ROLLBACK TRANSACTION @tran
		print error_message()
		IF(@procedure_name_nv IS NULL)
		BEGIN
			SET @error_i = 50001
			SET @error_nv = CONCAT('{"error_id_i":"-',@error_i,'","procedure_name_nv":"',@procedure_name_nv,'","error_nv":"',ERROR_MESSAGE(),'"}');
			THROW @error_i,@error_nv,1;
		END 
	END CATCH
END

GO
/****** Object:  StoredProcedure [dbo].[usp_user_login]    Script Date: 2025-05-18 20:47:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[usp_user_login]
	@json_in_nv nvarchar(MAX),
	@json_out_nv nvarchar(MAX) out
AS
BEGIN
	DECLARE @error_i int, @error_nv nvarchar(MAX), @procedure_name_nv nvarchar(MAX),@msg_out nvarchar(max), @result int, @json_result_nv nvarchar(MAX);
	DECLARE @evento varchar(200)='Login';
	DECLARE @evento_descripcion varchar(200)='Iniciar sesión';
	DECLARE @usuario_id int;

	SET @procedure_name_nv =(SELECT OBJECT_NAME(@@PROCID))

	DECLARE @json_in_schema_nv nvarchar(MAX)=N'{"usuario":"text","contrasena":"text","ip_client":"text"}';

	BEGIN TRY			

		IF(COALESCE(ISJSON(@json_in_nv),0)=0)
		BEGIN
			SET @error_i = 50100
			SET @error_nv = CONCAT('{"procedure_name_nv":"',@procedure_name_nv,'","error_nv":"El json no es valido.","error_id_i":"-',@error_i,'","json_in_nv_proc":',@json_in_nv,'}');
			THROW @error_i,@error_nv,1;
		END
		ELSE
		BEGIN 
			--AQUI METEMOS EN VARIABLES LOS VALORES QUE VENGAN EN EL @json_in_nv
			DECLARE @ip_client varchar(50)=isnull(CAST(JSON_VALUE(@json_in_nv, '$.ip_client')as varchar(50)),'0');
			DECLARE @usuario varchar(16)=CAST(JSON_VALUE(@json_in_nv, '$.usuario')as varchar(16));
			DECLARE @contrasena varchar(255)=CAST(JSON_VALUE(@json_in_nv, '$.contrasena')as varchar(255));	
			

			select @usuario_id=id 
			from tbUsuarios (NOLOCK)
			where usuario=@usuario	
			and contrasena=@contrasena
			AND ISNULL(estado,0)=1

			IF ISNULL(@usuario_id,0)=0
			BEGIN
				SET @error_i = 60001 
				SET @error_nv = CONCAT('{"procedure_name_nv":"',@procedure_name_nv,'","error_nv":"El usuario no ha sido encontrado","error_id_i":"-',@error_i,'","json_in_nv_proc":',@json_in_nv,'}');
				THROW @error_i,@error_nv,1;
			END
			ELSE
			BEGIN
				SET @json_result_nv=(SELECT  id,nombre,usuario,rol,fecha_creacion,estado from tbUsuarios where id=@usuario_id FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)				
				SET @result=1	
			END
		
			--AQUI CONCLUYE LA FUNCIONALIDAD-------------------------------------------------------------------------------------------------------------------------------------------------------------------									
			SET @json_out_nv=(SELECT @result result, JSON_QUERY(@json_result_nv) as json_result_nv, JSON_QUERY(@json_in_schema_nv) json_in_schema_nv, NULL as error_nv FOR JSON PATH, INCLUDE_NULL_VALUES, WITHOUT_ARRAY_WRAPPER)
			
		END
	END TRY
	BEGIN CATCH
		print error_message()
		IF(@procedure_name_nv IS NULL)
		BEGIN
			SET @error_i = 50001
			SET @error_nv = CONCAT('{"error_id_i":"-',@error_i,'","procedure_name_nv":"',@procedure_name_nv,'","error_nv":"',ERROR_MESSAGE(),'"}');
			THROW @error_i,@error_nv,1;
		END 
		
	END CATCH
END

GO
