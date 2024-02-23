
#!/bin/sh

EZYFOX_SERVER_HOME="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
PID_FILE=$EZYFOX_SERVER_HOME/.runtime/ezyfox_server_instance.pid

if [ ! -f "${PID_FILE}" ]; then
    echo "No ezyfox server instance is running."
    exit 0
fi

PID=$(cat "${PID_FILE}");
if [ -z "${PID}" ]; then
    echo "No ezyfox server instance is running."
    exit 0
else
   kill -15 "${PID}"
   rm "${PID_FILE}"
   echo "Ezyfox server with PID ${PID} shutdown."
   exit 0
fi
