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


	