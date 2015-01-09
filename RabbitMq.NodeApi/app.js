
/**
 * Module dependencies.
 */
var express = require('express');
var http = require('http');
var amqp = require('amqp');


/**
 * Web Server setup.
 */
var app = express();

app.set('port', process.env.PORT || 10025);

/**
 * Rabbit MQ connection setup
 */
var rabbitSettings = {
    mqUrl: process.env.RABBITMQ_URL || 'amqp://localhost',
    exchangeName: process.env.EXCHANGE || 'RabbitMQ.WebApi:SimpleMessage',
    queueName: process.env.QUEUE || 'apitest_webapi',
};


console.log('Opening connection to RabbitMQ');
var connection = amqp.createConnection({ url: rabbitSettings.mqUrl }, {
    reconnect: true, // Enable reconnection
    reconnectBackoffStrategy: 'linear',
    reconnectBackoffTime: 1000, // Try reconnect once a second
});

var exchange;

connection.on('ready', function () {
    var options = { type: 'fanout', durable: true, autoDelete: false }

    console.log('Creating/opening exchange');
    exchange = connection.exchange(rabbitSettings.exchangeName, options, function (exc) {

        console.log('Creating/opening queue');
        connection.queue(rabbitSettings.queueName, options, function (queue) {

            console.log('Binding queue to exchange');
            queue.bind(exc, queue.name);
        });
    });
});


/**
 * A RESTful GET request handler
 */
app.get('/Message/:message', function(request, response) {

    var message = request.params.message;

    var publishMessage = {
        'message': {
            'content': message
        },
        'messageType': [
            'urn:message:RabbitMQ.WebApi:SimpleMessage'
        ],
    };

    var publishOptions = {
        'headers': {
            'Content-Type': "application/vnd.masstransit+json"
        }
    };

    exchange.publish(rabbitSettings.queueName, publishMessage, publishOptions);

    response.writeHead(202);
    response.end();
});

/**
 * Start the web server
 */
http.createServer(app).listen(app.get('port'), function () {
    console.log('Express server listening on port ' + app.get('port'));
});

