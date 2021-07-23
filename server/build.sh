#export EZYFOX_SERVER_HOME=
mvn -pl . clean install
mvn -pl EzyTank-common -Pexport clean install
mvn -pl EzyTank-app-api -Pexport clean install
mvn -pl EzyTank-app-entry -Pexport clean install
mvn -pl EzyTank-plugin -Pexport clean install