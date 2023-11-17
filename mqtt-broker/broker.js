"use strict";

const aedes = require('aedes')();

const options = {
    clean: true
  }

const server = require('net').createServer(options, aedes.handle);

const Port = 1883;

// クライアントエラー.
aedes.on('clientError', (client, error) => {
    console.error('client error', client.id);
    console.error(error);
})

// 接続エラー.
aedes.on('connectionError', (client, error) => {
    console.error('connection error', client.id);
    console.error(error);
});

// publish.
aedes.on('publish', (packet, client) => {
    //console.log('publish', client);
});

// subscribe.
aedes.on('subscribe', (subscriptions, client) => {
    console.log('subscribe', client.id);
});

// subscribe 解除.
aedes.on('unsubscribe', (unsubscriptions, client) => {
    console.log('unsubscribe', client.id);
});

// 新規クライアントが登録.
aedes.on('client', (client) => {
    console.log('client', client.id);
});

// クライアントの切断.
aedes.on('clientDisconnect', (client) => {
    console.log('clientDisconnect', client.id);
});

// MQTT Brokerの起動.
server.listen(Port, () => {
    console.log('MQTT Broker start');
});
