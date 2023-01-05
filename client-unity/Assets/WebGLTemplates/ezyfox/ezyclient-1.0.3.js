//================ ezy-configs.js ================
var EzyClientConfig = function () {
    this.zoneName = '';
    this.clientName = '';
    this.reconnect = new EzyReconnectConfig();

    this.getClientName = function () {
        if (this.clientName == '') return this.zoneName;
        return this.clientName;
    };
};

var EzyReconnectConfig = function () {
    this.enable = true;
    this.maxReconnectCount = 5;
    this.reconnectPeriod = 3000;
};

//================ ezy-setup.js ================
var EzySetup = function (handlerManager) {
    this.handlerManager = handlerManager;
    this.appSetups = {};
    this.pluginSetups = {};

    this.addDataHandler = function (cmd, dataHandler) {
        this.handlerManager.addDataHandler(cmd, dataHandler);
        return this;
    };

    this.addEventHandler = function (eventType, eventHandler) {
        this.handlerManager.addEventHandler(eventType, eventHandler);
        return this;
    };

    this.setupApp = function (appName) {
        var appSetup = this.appSetups[appName];
        if (!appSetup) {
            var appDataHandlers =
                this.handlerManager.getAppDataHandlers(appName);
            appSetup = new EzyAppSetup(appDataHandlers, this);
            this.appSetups[appName] = appSetup;
        }
        return appSetup;
    };

    this.setupPlugin = function (pluginName) {
        var pluginSetup = this.pluginSetups[pluginName];
        if (!pluginSetup) {
            var pluginDataHandlers =
                this.handlerManager.getPluginDataHandlers(pluginName);
            pluginSetup = new EzyPluginSetup(pluginDataHandlers, this);
            this.pluginSetups[pluginName] = pluginSetup;
        }
        return pluginSetup;
    };

    this.setStreamingHandler = function (streamingHandler) {
        this.handlerManager.streamingHandler = streamingHandler;
    };
};

var EzyAppSetup = function (dataHandlers, parent) {
    this.parent = parent;
    this.dataHandlers = dataHandlers;

    this.addDataHandler = function (cmd, dataHandler) {
        this.dataHandlers.addHandler(cmd, dataHandler);
        return this;
    };

    this.done = function () {
        return this.parent;
    };
};

var EzyPluginSetup = function (dataHandlers, parent) {
    this.parent = parent;
    this.dataHandlers = dataHandlers;

    this.addDataHandler = function (cmd, dataHandler) {
        this.dataHandlers.addHandler(cmd, dataHandler);
        return this;
    };

    this.done = function () {
        return this.parent;
    };
};

//================ ezy-constants.js ================
var MAX_SMALL_MESSAGE_SIZE = 65535;

var EzyCommand = EzyCommand || {
    ERROR: { id: 10, name: 'ERROR' },
    HANDSHAKE: { id: 11, name: 'HANDSHAKE' },
    PING: { id: 12, name: 'PING' },
    PONG: { id: 13, name: 'PONG' },
    DISCONNECT: { id: 14, name: 'DISCONNECT' },
    LOGIN: { id: 20, name: 'LOGIN' },
    LOGIN_ERROR: { id: 21, name: 'LOGIN_ERROR' },
    LOGOUT: { id: 22, name: 'LOGOUT' },
    APP_ACCESS: { id: 30, name: 'APP_ACCESS' },
    APP_REQUEST: { id: 31, name: 'APP_REQUEST' },
    APP_EXIT: { id: 33, name: 'APP_EXIT' },
    APP_ACCESS_ERROR: { id: 34, name: 'APP_ACCESS_ERROR' },
    APP_REQUEST_ERROR: { id: 35, name: 'APP_REQUEST_ERROR' },
    PLUGIN_INFO: { id: 40, name: 'PLUGIN_INFO' },
    PLUGIN_REQUEST: { id: 41, name: 'PLUGIN_REQUEST' },
};

Object.freeze(EzyCommand);

var EzyCommands = EzyCommands || {};
EzyCommands[EzyCommand.ERROR.id] = EzyCommand.ERROR;
EzyCommands[EzyCommand.HANDSHAKE.id] = EzyCommand.HANDSHAKE;
EzyCommands[EzyCommand.PING.id] = EzyCommand.PING;
EzyCommands[EzyCommand.PONG.id] = EzyCommand.PONG;
EzyCommands[EzyCommand.DISCONNECT.id] = EzyCommand.DISCONNECT;
EzyCommands[EzyCommand.LOGIN.id] = EzyCommand.LOGIN;
EzyCommands[EzyCommand.LOGIN_ERROR.id] = EzyCommand.LOGIN_ERROR;
EzyCommands[EzyCommand.LOGOUT.id] = EzyCommand.LOGOUT;
EzyCommands[EzyCommand.APP_ACCESS.id] = EzyCommand.APP_ACCESS;
EzyCommands[EzyCommand.APP_REQUEST.id] = EzyCommand.APP_REQUEST;
EzyCommands[EzyCommand.APP_EXIT.id] = EzyCommand.APP_EXIT;
EzyCommands[EzyCommand.APP_ACCESS_ERROR.id] = EzyCommand.APP_ACCESS_ERROR;
EzyCommands[EzyCommand.APP_REQUEST_ERROR.id] = EzyCommand.APP_REQUEST_ERROR;
EzyCommands[EzyCommand.PLUGIN_INFO.id] = EzyCommand.PLUGIN_INFO;
EzyCommands[EzyCommand.PLUGIN_REQUEST.id] = EzyCommand.PLUGIN_REQUEST;

Object.freeze(EzyCommands);

var EzyEventType = EzyEventType || {
    CONNECTION_SUCCESS: 'CONNECTION_SUCCESS',
    CONNECTION_FAILURE: 'CONNECTION_FAILURE',
    DISCONNECTION: 'DISCONNECTION',
    LOST_PING: 'LOST_PING',
    TRY_CONNECT: 'TRY_CONNECT',
};

Object.freeze(EzyEventType);

var EzyConnectionStatus = EzyConnectionStatus || {
    NULL: 'NULL',
    CONNECTING: 'CONNECTING',
    CONNECTED: 'CONNECTED',
    DISCONNECTED: 'DISCONNECTED',
    FAILURE: 'FAILURE',
    RECONNECTING: 'RECONNECTING',
};

Object.freeze(EzyConnectionStatus);

var EzyConnectionFailedReason = EzyConnectionFailedReason || {
    NETWORK_UNREACHABLE: 'NETWORK_UNREACHABLE',
    UNKNOWN_HOST: 'UNKNOWN_HOST',
    CONNECTION_REFUSED: 'CONNECTION_REFUSED',
    UNKNOWN: 'UNKNOWN',
};

Object.freeze(EzyConnectionFailedReason);

var EzyDisconnectReason = EzyDisconnectReason || {
    CLOSE: -1,
    UNKNOWN: 0,
    IDLE: 1,
    NOT_LOGGED_IN: 2,
    ANOTHER_SESSION_LOGIN: 3,
    ADMIN_BAN: 4,
    ADMIN_KICK: 5,
    MAX_REQUEST_PER_SECOND: 6,
    MAX_REQUEST_SIZE: 7,
    SERVER_ERROR: 8,
    SERVER_NOT_RESPONDING: 400,
    UNAUTHORIZED: 401,
};

Object.freeze(EzyDisconnectReason);

var EzyDisconnectReasonNames = EzyDisconnectReasonNames || {};
EzyDisconnectReasonNames[EzyDisconnectReason.CLOSE] = 'CLOSE';
EzyDisconnectReasonNames[EzyDisconnectReason.UNKNOWN] = 'UNKNOWN';
EzyDisconnectReasonNames[EzyDisconnectReason.IDLE] = 'IDLE';
EzyDisconnectReasonNames[EzyDisconnectReason.NOT_LOGGED_IN] = 'NOT_LOGGED_IN';
EzyDisconnectReasonNames[EzyDisconnectReason.ANOTHER_SESSION_LOGIN] =
    'ANOTHER_SESSION_LOGIN';
EzyDisconnectReasonNames[EzyDisconnectReason.ADMIN_BAN] = 'ADMIN_BAN';
EzyDisconnectReasonNames[EzyDisconnectReason.ADMIN_KICK] = 'ADMIN_KICK';
EzyDisconnectReasonNames[EzyDisconnectReason.MAX_REQUEST_PER_SECOND] =
    'MAX_REQUEST_PER_SECOND';
EzyDisconnectReasonNames[EzyDisconnectReason.MAX_REQUEST_SIZE] =
    'MAX_REQUEST_SIZE';
EzyDisconnectReasonNames[EzyDisconnectReason.SERVER_ERROR] = 'SERVER_ERROR';
EzyDisconnectReasonNames[EzyDisconnectReason.SERVER_NOT_RESPONDING] =
    'SERVER_NOT_RESPONSE';
EzyDisconnectReasonNames[EzyDisconnectReason.UNAUTHORIZED] = 'UNAUTHORIZED';

EzyDisconnectReasonNames.parse = function (reasonId) {
    var name = EzyDisconnectReasonNames[reasonId];
    if (name) return name;
    return reasonId.toString();
};

Object.freeze(EzyDisconnectReasonNames);

//================ ezy-event-message-handler.js ================
var EzyEventMessageHandler = function (client) {
    this.client = client;
    this.handlerManager = client.handlerManager;
    this.unloggableCommands = client.unloggableCommands;

    this.handleEvent = function (event) {
        var eventHandler = this.handlerManager.getEventHandler(event.getType());
        if (eventHandler) eventHandler.handle(event);
        else EzyLogger.console('has no handler with event: ' + event.getType());
    };

    this.handleDisconnection = function (reason) {
        this.client.onDisconnected(reason);
        var event = new EzyDisconnectionEvent(reason);
        this.handleEvent(event);
    };

    this.handleMessage = function (message) {
        var cmd = EzyCommands[message[0]];
        var data = message.length > 1 ? message[1] : [];
        if (!this.unloggableCommands.includes(cmd))
            EzyLogger.console(
                'received cmd: ' + cmd.name + ', data: ' + JSON.stringify(data)
            );
        if (cmd === EzyCommand.DISCONNECT) this.handleDisconnectionData(data);
        else this.handleResponseData(cmd, data);
    };

    this.handleStreaming = function (bytes) {
        var streamingHandler = this.handlerManager.streamingHandler;
        streamingHandler.handle(bytes);
    };

    this.handleDisconnectionData = function (resonseData) {
        var reason = resonseData[0];
        this.handleDisconnection(reason);
    };

    this.handleResponseData = function (cmd, responseData) {
        var handler = this.handlerManager.getDataHandler(cmd);
        if (handler) handler.handle(responseData);
        else EzyLogger.console('has no handler with command: ' + cmd.name);
    };
};

//================ ezy-managers.js ================
var EzyAppManager = function (zoneName) {
    this.zoneName = zoneName;
    this.appList = [];
    this.appsById = {};
    this.appsByName = {};

    this.getApp = function () {
        var app = null;
        if (this.appList.length > 0) app = this.appList[0];
        else EzyLogger.console('has no app in zone: ' + this.zoneName);
        return app;
    };

    this.addApp = function (app) {
        this.appList.push(app);
        this.appsById[app.id] = app;
        this.appsByName[app.name] = app;
    };

    this.removeApp = function (appId) {
        var app = this.appsById[appId];
        if (app) {
            delete this.appsById[appId];
            delete this.appsByName[app.name];
            this.appList = this.appList.filter(function (item) {
                return item.id != appId;
            });
        }
        return app;
    };

    this.getAppById = function (id) {
        var app = this.appsById[id];
        return app;
    };

    this.getAppByName = function (name) {
        var app = this.appsByName[name];
        return app;
    };
};

//======================================

var EzyPluginManager = function (zoneName) {
    this.zoneName = zoneName;
    this.pluginList = [];
    this.pluginsById = {};
    this.pluginsByName = {};

    this.getPlugin = function () {
        var plugin = null;
        if (this.pluginList.length > 0) plugin = this.pluginList[0];
        else EzyLogger.console('has no plugin in zone: ' + this.zoneName);
        return plugin;
    };

    this.addPlugin = function (plugin) {
        this.pluginList.push(plugin);
        this.pluginsById[plugin.id] = plugin;
        this.pluginsByName[plugin.name] = plugin;
    };

    this.getPluginById = function (id) {
        var plugin = this.pluginsById[id];
        return plugin;
    };

    this.getPluginByName = function (name) {
        var plugin = this.pluginsByName[name];
        return plugin;
    };
};

//======================================

var EzyPingManager = function () {
    this.pingPeriod = 5000;
    this.lostPingCount = 0;
    this.maxLostPingCount = 5;

    this.increaseLostPingCount = function () {
        return ++this.lostPingCount;
    };
};

//======================================
var EzyStreamingHandler = function () {
    this.handle = function (bytes) {};
};

//======================================

var EzyHandlerManager = function (client) {
    this.streamingHandler = new EzyStreamingHandler();
    this.streamingHandler.client = client;

    this.newEventHandlers = function () {
        var handlers = new EzyEventHandlers(this.client);
        handlers.addHandler(
            EzyEventType.CONNECTION_SUCCESS,
            new EzyConnectionSuccessHandler()
        );
        handlers.addHandler(
            EzyEventType.CONNECTION_FAILURE,
            new EzyConnectionFailureHandler()
        );
        handlers.addHandler(
            EzyEventType.DISCONNECTION,
            new EzyDisconnectionHandler()
        );
        return handlers;
    };

    this.newDataHandlers = function () {
        var handlers = new EzyDataHandlers(this.client);
        handlers.addHandler(EzyCommand.PONG, new EzyPongHandler());
        handlers.addHandler(EzyCommand.HANDSHAKE, new EzyHandshakeHandler());
        handlers.addHandler(EzyCommand.LOGIN, new EzyLoginSuccessHandler());
        handlers.addHandler(EzyCommand.LOGIN_ERROR, new EzyLoginErrorHandler());
        handlers.addHandler(EzyCommand.APP_ACCESS, new EzyAppAccessHandler());
        handlers.addHandler(
            EzyCommand.APP_REQUEST,
            new EzyAppResponseHandler()
        );
        handlers.addHandler(EzyCommand.APP_EXIT, new EzyAppExitHandler());
        handlers.addHandler(EzyCommand.PLUGIN_INFO, new EzyPluginInfoHandler());
        handlers.addHandler(
            EzyCommand.PLUGIN_REQUEST,
            new EzyPluginResponseHandler()
        );
        return handlers;
    };

    this.getDataHandler = function (cmd) {
        var handler = this.dataHandlers.getHandler(cmd);
        return handler;
    };

    this.getEventHandler = function (eventType) {
        var handler = this.eventHandlers.getHandler(eventType);
        return handler;
    };

    this.getAppDataHandlers = function (appName) {
        var answer = this.appDataHandlerss[appName];
        if (!answer) {
            answer = new EzyAppDataHandlers();
            this.appDataHandlerss[appName] = answer;
        }
        return answer;
    };

    this.getPluginDataHandlers = function (pluginName) {
        var answer = this.pluginDataHandlerss[pluginName];
        if (!answer) {
            answer = new EzyPluginDataHandlers();
            this.pluginDataHandlerss[pluginName] = answer;
        }
        return answer;
    };

    this.addDataHandler = function (cmd, dataHandler) {
        this.dataHandlers.addHandler(cmd, dataHandler);
    };

    this.addEventHandler = function (eventType, eventHandler) {
        this.eventHandlers.addHandler(eventType, eventHandler);
    };

    this.setStreamingHandler = function (streamingHandler) {
        this.streamingHandler = streamingHandler;
        this.streamingHandler.client = this.client;
    };

    this.client = client;
    this.dataHandlers = this.newDataHandlers();
    this.eventHandlers = this.newEventHandlers();
    this.appDataHandlerss = {};
    this.pluginDataHandlerss = {};
};

//================ ezy-event-handlers.js ================
var EzyConnectionSuccessHandler = function () {
    this.clientType = 'JAVASCRIPT';
    this.clientVersion = '1.0.0';

    this.handle = function () {
        this.sendHandshakeRequest();
        this.postHandle();
    };

    this.postHandle = function () {};

    this.sendHandshakeRequest = function () {
        var request = this.newHandshakeRequest();
        this.client.sendRequest(EzyCommand.HANDSHAKE, request);
    };

    this.newHandshakeRequest = function () {
        var clientId = this.getClientId();
        var clientKey = this.getClientKey();
        var enableEncryption = this.isEnableEncryption();
        var token = this.getStoredToken();
        var request = [
            clientId,
            clientKey,
            this.clientType,
            this.clientVersion,
            enableEncryption,
            token,
        ];
        return request;
    };

    this.getClientKey = function () {
        return '';
    };

    this.getClientId = function () {
        var guid = EzyGuid.generate();
        return guid;
    };

    this.isEnableEncryption = function () {
        return false;
    };

    this.getStoredToken = function () {
        return '';
    };
};

//======================================

var EzyConnectionFailureHandler = function () {
    this.handle = function (event) {
        EzyLogger.console('connection failure, reason = ' + event.reason);
        var config = this.client.config;
        var reconnectConfig = config.reconnect;
        var should = this.shouldReconnect(event);
        var must = reconnectConfig.enable && should;
        var reconnecting = false;
        this.client.status = EzyConnectionStatus.FAILURE;
        if (must) reconnecting = this.client.reconnect();
        if (!reconnecting) {
            this.control(event);
        }
    };

    this.shouldReconnect = function (event) {
        return true;
    };

    this.control = function (event) {};
};

//======================================

var EzyDisconnectionHandler = function () {
    this.handle = function (event) {
        var reason = event.reason;
        var reasonName = EzyDisconnectReasonNames.parse(reason);
        EzyLogger.console('handle disconnection, reason = ' + reasonName);
        this.preHandle(event);
        var config = this.client.config;
        var reconnectConfig = config.reconnect;
        var should = this.shouldReconnect(event);
        var mustReconnect =
            reconnectConfig.enable &&
            reason != EzyDisconnectReason.UNAUTHORIZED &&
            reason != EzyDisconnectReason.CLOSE &&
            should;
        var reconnecting = false;
        this.client.status = EzyConnectionStatus.DISCONNECTED;
        if (mustReconnect) reconnecting = this.client.reconnect();
        if (!reconnecting) {
            this.control(event);
        }
        this.postHandle(event);
    };

    this.preHandle = function (event) {};

    this.shouldReconnect = function (event) {
        var reason = event.reason;
        if (reason == EzyDisconnectReason.ANOTHER_SESSION_LOGIN) return false;
        return true;
    };

    this.control = function (event) {};

    this.postHandle = function (event) {};
};

//======================================

var EzyEventHandlers = function (client) {
    this.handlers = {};
    this.client = client;
    this.pingSchedule = client.pingSchedule;

    this.addHandler = function (eventType, handler) {
        handler.client = this.client;
        handler.pingSchedule = this.pingSchedule;
        this.handlers[eventType] = handler;
    };

    this.getHandler = function (eventType) {
        var handler = this.handlers[eventType];
        return handler;
    };
};

//================ ezy-clients.js ================
var EzyClients = (function () {
    var EzyClientsClass = function () {
        this.clients = {};
        this.defaultClientName = '';

        this.newClient = function (config) {
            var client = new EzyClient(config);
            this.addClient(client);
            if (this.defaultClientName == '')
                this.defaultClientName = client.name;
            return client;
        };

        this.newDefaultClient = function (config) {
            var client = this.newClient(config);
            this.defaultClientName = client.name;
            return client;
        };

        this.addClient = function (client) {
            this.clients[client.name] = client;
        };

        this.getClient = function (clientName) {
            return this.clients[clientName];
        };

        this.getDefaultClient = function () {
            return this.clients[this.defaultClientName];
        };
    };

    var instance = null;

    return {
        getInstance: function () {
            if (!instance) {
                instance = new EzyClientsClass();
            }
            return instance;
        },
    };
})();

//================ ezy-client.js ================
var EzyConnector = function () {
    this.ws = null;
    this.destroyed = false;
    this.disconnectReason = null;

    this.connect = function (client, url) {
        this.ws = new WebSocket(url);
        var thiz = this;
        var failed = false;
        var pingManager = client.pingManager;
        var eventMessageHandler = client.eventMessageHandler;

        this.ws.onerror = function (e) {
            EzyLogger.console(
                'connect to: ' + url + ' error : ' + JSON.stringify(e)
            );
            failed = true;
            var event = new EzyConnectionFailureEvent(
                EzyConnectionFailedReason.UNKNOWN
            );
            eventMessageHandler.handleEvent(event);
        };

        this.ws.onopen = function () {
            EzyLogger.console('connected to: ' + url);
            client.reconnectCount = 0;
            client.status = EzyConnectionStatus.CONNECTED;
            var event = new EzyConnectionSuccessEvent();
            eventMessageHandler.handleEvent(event);
        };

        this.ws.onclose = function () {
            if (failed) return;
            if (thiz.destroyed) return;
            if (client.isConnected()) {
                var reason =
                    thiz.disconnectReason || EzyDisconnectReason.UNKNOWN;
                eventMessageHandler.handleDisconnection(reason);
            } else {
                EzyLogger.console(
                    'connection to: ' + url + ' has disconnected before'
                );
            }
        };

        this.ws.onmessage = function (event) {
            if (thiz.destroyed) return;
            pingManager.lostPingCount = 0;
            var data = event.data;
            if (typeof data === 'string') {
                handleTextMessage(data);
            } else {
                handleBinaryMessage(data);
            }
        };

        var handleTextMessage = function (data) {
            var message = JSON.parse(data);
            eventMessageHandler.handleMessage(message);
        };

        var handleBinaryMessage = function (bytes) {
            var arrayBuffer;
            var fileReader = new FileReader();
            fileReader.onload = function (event) {
                arrayBuffer = event.target.result;
                var uint8ArrayNew = new Uint8Array(arrayBuffer);
                var headerByte = uint8ArrayNew[0];
                var isRawBytes = (headerByte & (1 << 4)) != 0;
                if (isRawBytes) {
                    var isBigSize = (headerByte & (1 << 0)) != 0;
                    var offset = isBigSize ? 1 + 4 : 1 + 2;
                    var contentBytes = bytes.slice(offset);
                    eventMessageHandler.handleStreaming(contentBytes);
                } else {
                    // nerver fire, maybe server error
                }
            };
            fileReader.readAsArrayBuffer(bytes.slice(0, 1));
        };
    };

    this.disconnect = function (reason) {
        if (this.ws) {
            this.disconnectReason = reason;
            this.ws.close();
        }
    };

    this.destroy = function () {
        this.destroyed = true;
        this.disconnect();
    };

    this.send = function (data) {
        var json = JSON.stringify(data);
        this.ws.send(json);
    };

    this.sendBytes = function (bytes) {
        this.ws.send(bytes);
    };
};

//===============================================

var EzyClient = function (config) {
    this.config = config;
    this.name = config.getClientName();
    this.url = null;
    this.connector = null;
    this.zone = null;
    this.me = null;
    this.status = EzyConnectionStatus.NULL;
    this.reconnectCount = 0;
    this.disconnected = false;
    this.reconnectTimeout = null;
    this.pingManager = new EzyPingManager();
    this.pingSchedule = new EzyPingSchedule(this);
    this.handlerManager = new EzyHandlerManager(this);
    this.setup = new EzySetup(this.handlerManager);
    this.unloggableCommands = [EzyCommand.PING, EzyCommand.PONG];
    this.eventMessageHandler = new EzyEventMessageHandler(this);
    this.pingSchedule.eventMessageHandler = this.eventMessageHandler;

    this.connect = function (url) {
        this.url = url ? url : this.url;
        this.preconnect();
        this.reconnectCount = 0;
        this.status = EzyConnectionStatus.CONNECTING;
        this.connector = new EzyConnector();
        this.connector.connect(this, this.url);
    };

    this.reconnect = function () {
        var reconnectConfig = this.config.reconnect;
        var maxReconnectCount = reconnectConfig.maxReconnectCount;
        if (this.reconnectCount >= maxReconnectCount) return false;
        this.preconnect();
        this.status = EzyConnectionStatus.RECONNECTING;
        function reconnectNow(thiz) {
            thiz.reconnectTimeout = setTimeout(function () {
                thiz.connector = new EzyConnector();
                thiz.connector.connect(thiz, thiz.url);
            }, reconnectConfig.reconnectPeriod);
        }
        reconnectNow(this);
        this.reconnectCount++;
        var event = new EzyTryConnectEvent(this.reconnectCount);
        this.eventMessageHandler.handleEvent(event);
    };

    this.preconnect = function () {
        this.zone = null;
        this.me = null;
        this.appsById = {};
        if (this.connector) this.connector.destroy();
        if (this.reconnectTimeout) clearTimeout(this.reconnectTimeout);
    };

    this.disconnect = function (reason) {
        var actualReason = reason || Const.EzyDisconnectReason.CLOSE;
        this.internalDisconnect(reason);
    };

    this.close = function () {
        this.disconnect();
    };

    this.internalDisconnect = function (reason) {
        if (this.connector) this.connector.disconnect(reason);
    };

    this.sendBytes = function (bytes) {
        this.connector.sendBytes(bytes);
    };

    this.send = function (cmd, data) {
        this.sendRequest(cmd, data);
    };

    this.sendRequest = function (cmd, data) {
        if (!this.unloggableCommands.includes(cmd)) {
            EzyLogger.console(
                'send cmd: ' + cmd.name + ', data: ' + JSON.stringify(data)
            );
        }
        var request = [cmd.id, data];
        this.connector.send(request);
    };

    this.onDisconnected = function (reason) {
        this.status = EzyConnectionStatus.DISCONNECTED;
        this.pingSchedule.stop();
        this.internalDisconnect();
    };

    this.isConnected = function () {
        var connected = this.status == EzyConnectionStatus.CONNECTED;
        return connected;
    };

    this.getApp = function () {
        if (!this.zone) return null;
        var appManager = this.zone.appManager;
        return appManager.getApp();
    };

    this.getAppById = function (appId) {
        if (!this.zone) return null;
        var appManager = this.zone.appManager;
        return appManager.getAppById(appId);
    };

    this.getPlugin = function (pluginId) {
        if (!this.zone) return null;
        var pluginManager = this.zone.pluginManager;
        return pluginManager.getPlugin();
    };

    this.getPluginById = function (pluginId) {
        if (!this.zone) return null;
        var pluginManager = this.zone.pluginManager;
        return pluginManager.getPluginById(pluginId);
    };

    this.getAppManager = function () {
        if (!this.zone) return null;
        return this.zone.appManager;
    };

    this.getPluginManager = function () {
        if (!this.zone) return null;
        return this.zone.pluginManager;
    };
};

//================ ezy-events.js ================
var EzyConnectionSuccessEvent = function () {
    this.getType = function () {
        return EzyEventType.CONNECTION_SUCCESS;
    };
};

var EzyTryConnectEvent = function (count) {
    this.count = count;

    this.getType = function () {
        return EzyEventType.TRY_CONNECT;
    };
};

var EzyConnectionFailureEvent = function (reason) {
    this.reason = reason;

    this.getType = function () {
        return EzyEventType.CONNECTION_FAILURE;
    };
};

var EzyLostPingEvent = function (count) {
    this.count = count;

    this.getType = function () {
        return EzyEventType.LOST_PING;
    };
};

var EzyDisconnectionEvent = function (reason) {
    this.reason = reason;

    this.getType = function () {
        return EzyEventType.DISCONNECTION;
    };
};

//================ ezy-entities.js ================
var EzyUser = function (id, name) {
    this.id = id;
    this.name = name;
};

//===================================================

var EzyZone = function (client, id, name) {
    this.id = id;
    this.name = name;
    this.client = client;
    this.appManager = new EzyAppManager(name);
    this.pluginManager = new EzyPluginManager(name);
};

//===================================================

var EzyApp = function (client, zone, id, name) {
    this.id = id;
    this.name = name;
    this.client = client;
    this.zone = zone;
    this.dataHandlers = client.handlerManager.getAppDataHandlers(name);

    this.send = function (cmd, data) {
        this.sendRequest(cmd, data);
    };

    this.sendRequest = function (cmd, data) {
        var validData = data;
        if (!validData) validData = {};
        var requestData = [this.id, [cmd, validData]];
        this.client.sendRequest(EzyCommand.APP_REQUEST, requestData);
    };

    this.getDataHandler = function (cmd) {
        var handler = this.dataHandlers.getHandler(cmd);
        return handler;
    };
};

//===================================================

var EzyPlugin = function (client, zone, id, name) {
    this.id = id;
    this.name = name;
    this.client = client;
    this.zone = zone;
    this.dataHandlers = client.handlerManager.getPluginDataHandlers(name);

    this.send = function (cmd, data) {
        this.sendRequest(cmd, data);
    };

    this.sendRequest = function (cmd, data) {
        var validData = data;
        if (!validData) validData = {};
        var requestData = [this.id, [cmd, validData]];
        this.client.sendRequest(EzyCommand.PLUGIN_REQUEST, requestData);
    };

    this.getDataHandler = function (cmd) {
        var handler = this.dataHandlers.getHandler(cmd);
        return handler;
    };
};

//================ ezy-data-handlers.js ================
var EzyHandshakeHandler = function () {
    this.handle = function (data) {
        this.startPing();
        this.handleLogin();
        this.postHandle(data);
    };

    this.postHandle = function (data) {};

    this.handleLogin = function () {
        var loginRequest = this.getLoginRequest();
        this.client.sendRequest(EzyCommand.LOGIN, loginRequest);
    };

    this.getLoginRequest = function () {
        return ['test', 'test', 'test', []];
    };

    this.startPing = function () {
        this.client.pingSchedule.start();
    };
};

//======================================

var EzyLoginSuccessHandler = function () {
    this.handle = function (data) {
        var zoneId = data[0];
        var zoneName = data[1];
        var userId = data[2];
        var username = data[3];
        var responseData = data[4];

        var zone = new EzyZone(this.client, zoneId, zoneName);
        var user = new EzyUser(userId, username);
        this.client.me = user;
        this.client.zone = zone;
        this.handleLoginSuccess(responseData);
        EzyLogger.console('user: ' + user.name + ' logged in successfully');
    };

    this.handleLoginSuccess = function (responseData) {};
};

//======================================

var EzyLoginErrorHandler = function () {
    this.handle = function (data) {
        this.client.disconnect(401);
        this.handleLoginError(data);
    };

    this.handleLoginError = function (data) {};
};

//======================================

var EzyAppAccessHandler = function () {
    this.handle = function (data) {
        var zone = this.client.zone;
        var appManager = zone.appManager;
        var app = this.newApp(zone, data);
        appManager.addApp(app);
        this.postHandle(app, data);
        EzyLogger.console('access app: ' + app.name + ' successfully');
    };

    this.newApp = function (zone, data) {
        var appId = data[0];
        var appName = data[1];
        var app = new EzyApp(this.client, zone, appId, appName);
        return app;
    };

    this.postHandle = function (app, data) {};
};

//======================================

var EzyAppExitHandler = function () {
    this.handle = function (data) {
        var zone = this.client.zone;
        var appManager = zone.appManager;
        var appId = data[0];
        var reasonId = data[1];
        var app = appManager.removeApp(appId);
        if (app) {
            this.postHandle(app, data);
            EzyLogger.console(
                'user exit app: ' + app.name + ', reason: ' + reasonId
            );
        }
    };

    this.postHandle = function (app, data) {};
};

//======================================

var EzyAppResponseHandler = function () {
    this.handle = function (data) {
        var appId = data[0];
        var responseData = data[1];
        var cmd = responseData[0];
        var commandData = responseData[1];

        var app = this.client.getAppById(appId);
        if (!app) {
            EzyLogger.console('receive message when has not joined app yet');
            return;
        }
        var handler = app.getDataHandler(cmd);
        if (handler) handler(app, commandData);
        else
            EzyLogger.console(
                'app: ' + app.name + ' has no handler for command: ' + cmd
            );
    };
};

//======================================

var EzyPluginInfoHandler = function () {
    this.handle = function (data) {
        var zone = this.client.zone;
        var pluginManager = zone.pluginManager;
        var plugin = this.newPlugin(zone, data);
        pluginManager.addPlugin(plugin);
        this.postHandle(plugin, data);
        EzyLogger.console(
            'request plugin: ' + plugin.name + ' info successfully'
        );
    };

    this.newPlugin = function (zone, data) {
        var pluginId = data[0];
        var pluginName = data[1];
        var plugin = new EzyPlugin(this.client, zone, pluginId, pluginName);
        return plugin;
    };

    this.postHandle = function (plugin, data) {};
};

//======================================

var EzyPluginResponseHandler = function () {
    this.handle = function (data) {
        var pluginId = data[0];
        var responseData = data[1];
        var cmd = responseData[0];
        var commandData = responseData[1];

        var plugin = this.client.getPluginById(pluginId);
        var handler = plugin.getDataHandler(cmd);
        if (handler) handler(plugin, commandData);
        else
            EzyLogger.console(
                'plugin: ' + plugin.name + ' has no handler for command: ' + cmd
            );
    };
};

//======================================

var EzyPongHandler = function () {
    this.handle = function (client) {};
};

//======================================

var EzyDataHandlers = function (client) {
    this.handlers = {};
    this.client = client;
    this.pingSchedule = client.pingSchedule;

    this.addHandler = function (cmd, handler) {
        handler.client = this.client;
        handler.pingSchedule = this.pingSchedule;
        this.handlers[cmd.id] = handler;
    };

    this.getHandler = function (cmd) {
        var handler = this.handlers[cmd.id];
        return handler;
    };
};

//======================================

var EzyAppDataHandlers = function () {
    this.handlers = {};

    this.addHandler = function (cmd, handler) {
        this.handlers[cmd] = handler;
    };

    this.getHandler = function (cmd) {
        var handler = this.handlers[cmd];
        return handler;
    };
};

//======================================

var EzyPluginDataHandlers = function () {
    this.handlers = {};

    this.addHandler = function (cmd, handler) {
        this.handlers[cmd] = handler;
    };

    this.getHandler = function (cmd) {
        var handler = this.handlers[cmd];
        return handler;
    };
};

//================ ezy-utilities.js ================
var EzyGuid = EzyGuid || function () {};

EzyGuid.generate = function () {
    function s4() {
        return Math.floor((1 + Math.random()) * 0x10000)
            .toString(16)
            .substring(1);
    }
    return (
        s4() +
        s4() +
        '-' +
        s4() +
        '-' +
        s4() +
        '-' +
        s4() +
        '-' +
        s4() +
        s4() +
        s4()
    );
};

var EzyCodecs = EzyCodecs || function () {};

EzyCodecs.getSizeLength = function (bytesLength) {
    if (bytesLength > MAX_SMALL_MESSAGE_SIZE) return 4;
    return 2;
};

EzyCodecs.getIntBytes = function (value, size) {
    var bytes = [];
    for (var i = 0; i < size; ++i) {
        var byteValue = (value >> ((size - i - 1) * 8)) & 0xff;
        bytes.push(byteValue);
    }
    return bytes;
};

var EzyLogger = EzyLogger || function () {};

EzyLogger.debug = true;
EzyLogger.console = function (message) {
    if (EzyLogger.debug) console.log(message);
};

//================ ezy-sockets.js ================
var EzyPingSchedule = function (client) {
    this.client = client;
    this.pingManager = client.pingManager;
    this.eventMessageHandler = null;

    this.start = function () {
        var startPingNow = function (thiz) {
            thiz.pingInterval = setInterval(function () {
                thiz.sendPingRequest();
            }, thiz.pingManager.pingPeriod);
        };
        this.stop();
        startPingNow(this);
    };

    this.sendPingRequest = function () {
        var maxLostPingCount = this.pingManager.maxLostPingCount;
        var lostPingCount = this.pingManager.increaseLostPingCount();
        if (lostPingCount >= maxLostPingCount) {
            var reason = EzyDisconnectReason.SERVER_NOT_RESPONDING;
            this.eventMessageHandler.handleDisconnection(reason);
        } else {
            this.client.sendRequest(EzyCommand.PING, []);
        }
    };

    this.stop = function () {
        if (this.pingInterval) clearInterval(this.pingInterval);
    };
};
