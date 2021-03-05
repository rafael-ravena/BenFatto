-- Role: "BenFattoUsr-CLF"
-- DROP ROLE "BenFattoUsr-CLF";

CREATE ROLE "BenFattoUsr-CLF" WITH
  LOGIN
  SUPERUSER
  INHERIT
  CREATEDB
  CREATEROLE
  REPLICATION
  ENCRYPTED PASSWORD 'md51d9a1d8254f8ac69eb684ffb409c44cb';
  
-- Role: "BenFattoUsr-FrontEnd"
-- DROP ROLE "BenFattoUsr-FrontEnd";

CREATE ROLE "BenFattoUsr-FrontEnd" WITH
  LOGIN
  SUPERUSER
  INHERIT
  CREATEDB
  CREATEROLE
  NOREPLICATION
  ENCRYPTED PASSWORD 'md556f6c562e4ba8b6e22cd37614a24bfec';
  