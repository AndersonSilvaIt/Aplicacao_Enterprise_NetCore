#aguardando 90 segundos para aguardar o provisionamento e start do banco
sleep 90s
#rodar o comando para criar o banco
/opt/mssql-tools/bin/sqlcmd -S localhost,1433 -U SA -P "abcd@1234" -i criacao-banco-docker.sql