using System;
using System.ComponentModel;

namespace FTP_Exam_Library
{
    public enum MinimalCommands
    {
        USER,
        QUIT,
        PORT,

        TYPE, // ASCII Non - print
        MODE, // Stream
        STRU, // File

        RETR, // download
        STOR, // upload

        CWD,  // Change Working Directory
        LIST
    }


    public enum Structures : byte
    {
        [Description("File (no record structure) (default structure)")]
        File = (byte)'F',

        [Description("Record structure")]
        Record = (byte)'R',

        [Description("Page structure")]
        Page = (byte)'P'
    }

    public enum RepresentationTypes
    {
        ASCII = (byte)'A',
        EBDCIC = (byte)'E'
    }

    public enum TransferModes
    {
        Stream = (byte)'S',
        Block = (byte)'B',
        Compressed = (byte)'C'
    }

    /*
    5.1.MINIMUM IMPLEMENTATION

         In order to make FTP workable without needless error messages, the
          following minimum implementation is required for all servers:

             TYPE - ASCII Non - print
             MODE - Stream
             STRUCTURE - File, Record
             COMMANDS - USER, QUIT, PORT,
                        TYPE, MODE, STRU,
                          for the default values
                        RETR, STOR,
                        NOOP.

          The default values for transfer parameters are:

             TYPE - ASCII Non - print
             MODE - Stream
             STRU - File

          All hosts must accept the above as the standard defaults.
    */


    public enum StatusCodes
    {
        //Undefined = 0,
        //RestartMarker = 110,
        //ServiceTemporarilyNotAvailable = 120,
      //  DataAlreadyOpen = 125,
        OpeningData = 150,
        CommandOK = 200,
        //CommandExtraneous = 202,
        //SystemHelpReply = 211,
        DirectoryStatus = 212,
        FileStatus = 213,
        //SystemType = 215,
        SendUserCommand = 220,
        ClosingControl = 221,
        ClosingData = 226,
        //EnteringPassive = 227,
        //EnteringExtendedPassive = 229,
        LoggedInProceed = 230,
        //ServerWantsSecureSession = 234,
        FileActionOK = 250,
        //PathnameCreated = 257,
        SendPasswordCommand = 331,
        //NeedLoginAccount = 332,
        FileCommandPending = 350,
        //ServiceNotAvailable = 421,
        //CantOpenData = 425,
        //ConnectionClosed = 426,
        //ActionNotTakenFileUnavailableOrBusy = 450,
        //ActionAbortedLocalProcessingError = 451,
        //ActionNotTakenInsufficientSpace = 452,
        //CommandSyntaxError = 500,
        //ArgumentSyntaxError = 501,
        //CommandNotImplemented = 502,
        //BadCommandSequence = 503,
        //NotLoggedIn = 530,
        //AccountNeeded = 532,
        //ActionNotTakenFileUnavailable = 550,
        //ActionAbortedUnknownPageType = 551,
        //FileActionAborted = 552,
        //ActionNotTakenFilenameNotAllowed = 553
    }

}