FROM mcr.microsoft.com/mssql/server:2017-latest

USER root

COPY setupDB.sh setupDB.sh
COPY createDB.sql createDB.sql
COPY entrypoint.sh entrypoint.sh

RUN chmod +x entrypoint.sh

CMD /bin/bash ./entrypoint.sh