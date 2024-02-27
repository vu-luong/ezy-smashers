
#!/bin/sh

EZYFOX_SERVER_HOME="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
echo 'current dir: ' $EZYFOX_SERVER_HOME
CLASSPATH="lib/*:settings/:common/*:apps/common/*:apps/resources/*"
echo 'classpath: ' $CLASSPATH
java $1 -cp $CLASSPATH org.youngmonkeys.ezysmashers.ApplicationStartup
