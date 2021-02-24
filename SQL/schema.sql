-- DROP ROLE "BenFattoUsr-CLF";
CREATE ROLE "BenFattoUsr-CLF" WITH
  LOGIN
  SUPERUSER
  INHERIT
  CREATEDB
  CREATEROLE
  REPLICATION
  ENCRYPTED PASSWORD 'md51d9a1d8254f8ac69eb684ffb409c44cb';
  
-- DROP DATABASE "BenFatto-CLF";
CREATE DATABASE "BenFatto-CLF"
    WITH 
    OWNER = "BenFattoUsr-CLF"
    ENCODING = 'UTF8'
    LC_COLLATE = 'Portuguese_Brazil.1252'
    LC_CTYPE = 'Portuguese_Brazil.1252'
    TABLESPACE = pg_default
    CONNECTION LIMIT = -1;

-- DROP TABLE public."Import";
CREATE TABLE public."Import"
(
    "Id" serial NOT NULL,
    "When" date NOT NULL,
    "FileName" character varying(1024) COLLATE pg_catalog."default" NOT NULL,
    "MismatchRowsFileName" character varying(1024) COLLATE pg_catalog."default" NOT NULL,
    "UserId" Integer DEFAULT 0,
    "RowCount" integer DEFAULT 0,
    "ErrorCount" integer DEFAULT 0,
    "SuccessCount" integer DEFAULT 0,
    CONSTRAINT "Import_pkey" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;
ALTER TABLE public."Import"
    OWNER to "BenFattoUsr-CLF";
	
-- DROP TABLE public."LogRow";
CREATE TABLE public."LogRow"
(
    "Id" bigserial NOT NULL,
    "ImportId" integer NOT NULL,
    "IpAddress" character varying(15) COLLATE pg_catalog."default" NOT NULL,
    "RfcId" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "UserId" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Date" date NOT NULL,
    "TimeZone" smallint NOT NULL,
    "Method" smallint NOT NULL,
    "Resource" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Protocol" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "ResponseCode" smallint NOT NULL,
    "ResponseSize" bigint NOT NULL,
    "Referer" character varying(1024) COLLATE pg_catalog."default",
    "UserAgent" character varying(4096) COLLATE pg_catalog."default",
    "OriginalLine" character varying(8192) COLLATE pg_catalog."default" NOT NULL,
    "RowNumber" integer,
    CONSTRAINT "LogRow_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Import_LogRow" FOREIGN KEY ("ImportId")
        REFERENCES public."Import" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;
ALTER TABLE public."LogRow"
    OWNER to "BenFattoUsr-CLF";
	
-- DROP TABLE public."LogRowMismatch";
CREATE TABLE public."LogRowMismatch"
(
    "Id" bigserial NOT NULL,
    "ImportId" integer NOT NULL,
    "OriginalRowNumber" integer NOT NULL,
    "RowNumber" integer NOT NULL,
    "Row" character varying(8096) COLLATE pg_catalog."default" NOT NULL,
    "ThrownException" character varying(10240) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "LogRowMismatch_pkey" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Import_LogRowMismatch" FOREIGN KEY ("ImportId")
        REFERENCES public."Import" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;
ALTER TABLE public."LogRowMismatch"
    OWNER to "BenFattoUsr-CLF";	