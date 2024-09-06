


# Notification Service with Rate Limiting

  

  

This project implements a notification service using .NET 8 Minimal APIs, along with a custom rate-limiting mechanism.

  

  

## Features

  

  

-  **.NET 8**: Built using .NET 8 with Minimal APIs.

  

-  **Rate Limiting**: Custom rate-limiting implementation that limits notifications based on rules

  

  

> - [x] Status: not more than 2 per minute for each recipient

> - [x] News: not more than 1 per day for each recipient

> - [x] Marketing: not more than 3 per hour for each recipient

  

-  **Unit Tests**: Comprehensive unit tests for the rate-limiting service using xUnit and NSubstitute.

  

  

## Technologies

  

  

-  **.NET 8**

  

-  **Minimal APIs**

  

-  **Concurrent Collections**: Using `ConcurrentDictionary` and `ConcurrentQueue` for thread-safe operations.

  

-  **Unit Testing**: xUnit and NSubstitute for mocking and testing.

  

  

## Getting Started

  

  

### Prerequisites

  

  

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)

  

  

### Setup

1. Clone the repository:

       git clone https://github.com/arubesu/notification-service-app.git
       cd notification-service-app

2. Build the project:

       dotnet build

3. Run the application:

   dotnet run --project NotificationApp.API/

### Endpoints

#### Send Notification

- **Endpoint**: POST /api/send-notification
- **Description**: Sends a notification 

- **Request Body**:

      {
        "Type": "StatusUpdate",
        "UserId": "user1",
        "Message": "Message."
      }

- **Responses**:
  - **200 OK:** Notification sent successfully.
  - **400 Bad Request:** Validation errors.
  - **429 Too Many Requests:** Rate limit exceeded.
  - **500 Internal Server Error:** Unexpected errors.

### Example Usage

To send a notification, make a POST request to /api/send-notification with the following JSON payload:

       {
            "Type": "StatusUpdate",
            "UserId": "user1",
            "Message": "Message."
          }

### Rate Limiting

The service uses a custom rate-limiting implementation that enforces rules per notification type:

- StatusUpdate: Not more than 2 per minute.
- DailyNews: Not more than 1 per day.
- Marketing: Not more than 3 per hour.

If a rate limit is exceeded, a 429 Too Many Requests response is returned with a message indicating the rate limit has been exceeded.

### Unit Tests

Unit tests are provided to ensure that the rate-limiting rules are correctly enforced. Tests can be run using the following command:

    dotnet test

The tests cover scenarios such as:

- Rate limits being respected for different notification types.
- Rate limits applied to multiple users.
- Success and failure cases for sending notifications.

