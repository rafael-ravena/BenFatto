-- DROP TABLE public."Users";

CREATE TABLE public."Users"
(
    "Id" serial NOT NULL,
    "Name" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    email character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Password" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Users"
    OWNER to "BenFattoUsr-FrontEnd";	

-- DROP TABLE public."Functionalities";

CREATE TABLE public."Functionalities"
(
    "Id" serial NOT NULL,
    "Name" character varying(255) COLLATE pg_catalog."default" NOT NULL,
    "Route" character varying(1024) COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_Functionalities" PRIMARY KEY ("Id")
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."Functionalities"
    OWNER to "BenFattoUsr-FrontEnd";	

-- DROP TABLE public."UserFunctionalities";

CREATE TABLE public."UserFunctionalities"
(
    "UserId" integer NOT NULL,
    "FunctionalityId" integer NOT NULL,
    CONSTRAINT "PK_UserFunctionalities" PRIMARY KEY ("UserId", "FunctionalityId"),
    CONSTRAINT "FK_Functionalities_UserFunctionalities" FOREIGN KEY ("FunctionalityId")
        REFERENCES public."Functionalities" ("Id") MATCH SIMPLE
        ON UPDATE NO ACTION
        ON DELETE NO ACTION
        NOT VALID,
    CONSTRAINT "FK_Users_UserFunctionalities" FOREIGN KEY ("UserId")
        REFERENCES public."Users" ("Id") MATCH SIMPLE
        ON UPDATE CASCADE
        ON DELETE CASCADE
        NOT VALID
)
WITH (
    OIDS = FALSE
)
TABLESPACE pg_default;

ALTER TABLE public."UserFunctionalities"
    OWNER to "BenFattoUsr-FrontEnd";	
	