AVS CARD TEST CLIENT — README
========================================================

1. OVERVIEW
--------------------------------------------------------
AVS Card Test Client is a standalone WPF application used for testing 
and validating AVS Card transactions.

It allows developers and QA engineers to send BALANCE, REDEEM, REFUND, 
and CANCEL requests to the AVS Transaction Gateway and inspect responses 
in real time.

The application loads configuration from appsettings.test.json, supports 
structured logging, and keeps the UI responsive using asynchronous 
background execution.


2. FEATURES
--------------------------------------------------------
• Load AVS configuration from JSON
• Execute AVS operations:
    - BALANCE
    - REDEEM
    - REFUND
    - CANCEL
• Modal “Please wait…” dialog
• Optional semi-transparent overlay
• RichTextBox output log
• Structured logging via Microsoft.Extensions.Logging
• Automatic TxId generation
• Background execution to avoid UI freezing


3. CONFIGURATION FILE (appsettings.test.json)
--------------------------------------------------------
The application reads configuration from appsettings.test.json located 
in the output directory.

The file must be marked as:
- Build Action: Content
- Copy to Output Directory: Copy always

Example configuration:

{
  "Avs": {
    "Endpoint": "https://up.test.epayworldwide.com/tgxml",
    "Username": "test",
    "Password": "test",
    "TerminalId": "test",
    "TestPan": ";6364530000000000=49121011234?"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}


4. AVS CONFIGURATION PARAMETERS
--------------------------------------------------------

Avs.Endpoint
    URL of the AVS transaction gateway.
    Default test endpoint:
    https://up.test.epayworldwide.com/tgxml

Avs.Username
    AVS account username.

Avs.Password
    AVS account password.
    Loaded into ViewModel and applied to PasswordBox in code-behind.

Avs.TerminalId
    Terminal identifier used for all transactions.

Avs.TestPan
    Test PAN value.
    Supports track-2 format (e.g., ;PAN=EXP?).


5. LOGGING CONFIGURATION
--------------------------------------------------------
The Logging section controls verbosity of application logs.

Logging.LogLevel.Default
    Minimum log level for the application 
    (Information, Debug, Warning, Error, etc.)

Logging.LogLevel.Microsoft
    Overrides log level for Microsoft libraries.

Logs include:
    - Configuration loading
    - Request creation
    - Request execution
    - Response details
    - Validation errors
    - Exceptions


6. HOW LOGGING WORKS
--------------------------------------------------------
The application uses LoggerFactoryBuilder to create an ILoggerFactory:

    • Reads logging configuration from JSON
    • Applies log level settings
    • Enables console logging

Each ViewModel receives an ILogger<AvsClientTestViewModel> instance.

Example log output:

info: Configuration loaded. Endpoint=https://...
info: Executing AVS operation: BALANCE
debug: Creating AVS client for endpoint ...
info: Received AVS response: BALANCE, Result=0


7. RUNNING THE APPLICATION
--------------------------------------------------------
1. Ensure appsettings.test.json is present in the output folder.
2. Start the WPF application.
3. Adjust parameters if needed.
4. Click one of the operation buttons:
       BALANCE
       REDEEM
       REFUND
       CANCEL
5. Observe:
       - Modal wait dialog or overlay
       - RichTextBox output
       - Console logs


8. NOTES
--------------------------------------------------------
• PasswordBox does not support binding; password is applied manually 
  in code-behind.

• All AVS requests run on a background thread to keep UI responsive.

• This client is intended for testing and development only.
