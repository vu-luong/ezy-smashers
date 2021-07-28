#export EZYFOX_SERVER_HOME=
mvn -pl . clean install
mvn -pl EzySmashers-common -Pexport clean install
mvn -pl EzySmashers-app-api -Pexport clean install
mvn -pl EzySmashers-app-entry -Pexport clean install
mvn -pl EzySmashers-plugin -Pexport clean install