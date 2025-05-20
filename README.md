A notification api service for sending Email and SMS notifications using available providers, such as AWS Sns, Twilio, Vonage.
Providers can be configured, by being enabled/disabled, changed priority or removed/added.

It has a RabbitMq implementation using MassTransit nugget packaged to route messages to their corresponding handlers.
RabbitMq message queues can be added/removed in the RabbitMq.json file. 

Service uses .Net BackgroundService to resend all failed notifications if none of the providers were able to send it.

Run "docker-compose up" to get the latest docker image.
