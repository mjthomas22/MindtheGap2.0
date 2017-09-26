//var apiKey = 'AIzaSyAsOIuikEWJisvhZ-XctdMiikl0v2LUUfw';

// Client ID and API key from the Developer Console
var CLIENT_ID = '564506460648-knam6scga0e8i8ohtfs0246nnl4ffu39.apps.googleusercontent.com';

// Array of API discovery doc URLs for APIs used by the quickstart
var DISCOVERY_DOCS = ["https://www.googleapis.com/discovery/v1/apis/calendar/v3/rest"];

// Authorization scopes required by the API; multiple scopes can be
// included, separated by spaces.
var SCOPES = "https://www.googleapis.com/auth/calendar";

var authorizeButton = document.getElementById('authorize-button');
var signoutButton = document.getElementById('signout-button');
var gapsButton = document.getElementById('gaps-button');

/**
 *  On load, called to load the auth2 library and API client library.
 */
function handleClientLoad() {
    gapi.load('client:auth2', initClient);
}

/**
 *  Initializes the API client library and sets up sign-in state
 *  listeners.
 */
function initClient() {
    gapi.client.init({
        discoveryDocs: DISCOVERY_DOCS,
        clientId: CLIENT_ID,
        scope: SCOPES
    }).then(function () {
        // Listen for sign-in state changes.
        gapi.auth2.getAuthInstance().isSignedIn.listen(updateSigninStatus);

        // Handle the initial sign-in state.
        updateSigninStatus(gapi.auth2.getAuthInstance().isSignedIn.get());
        authorizeButton.onclick = handleAuthClick;
        signoutButton.onclick = handleSignoutClick;
    });
}

//*
// *  Called when the signed in status changes, to update the UI
// *  appropriately. After a sign-in, the API is called.

function updateSigninStatus(isSignedIn) {
    if (isSignedIn) {
        authorizeButton.style.display = 'none';
        signoutButton.style.display = 'block';
        listUpcomingEvents();
    } else {
        authorizeButton.style.display = 'block';
        signoutButton.style.display = 'none';
    }
}

//*
// *  Sign in the user upon button click.

function handleAuthClick(event) {
    gapi.auth2.getAuthInstance().signIn();
}

//*
// *  Sign out the user upon button click.

function handleSignoutClick(event) {
    gapi.auth2.getAuthInstance().signOut();
}

function listUpcomingEvents() {
    $.ajax({
        url: 'EventDuplicates',
    });
    gapi.client.calendar.events.list({
        'calendarId': 'primary',
        'timeMin': (new Date()).toISOString(),
        'showDeleted': false,
        'singleEvents': true,
        'orderBy': 'startTime'
    }).then(function (response) {
        var events = response.result.items;

        if (events.length > 0) {
            for (i = 0; i < events.length; i++) {
                var event = events[i];
                var when = event.start.dateTime;
                if (!when) {
                    when = "All Day";
                }
                //START OF TEST
                var data = {
                    EventSummary: event.summary,
                    EventLocation: event.location,
                    EventDescription: event.description,
                    EventStart: when,
                    EventEnd: event.end.dateTime,
                    EventRecurrence: event.recurrence,
                    EventReminders: event.reminders,
                    EventColor: event.colorId
                };
                //alert(data.EventSummary);
                $.ajax({
                    url: "GetUserEventInfo",
                    type: "POST",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(data),
                    success: function (mydata) {
                        alert(mydata);
                    }

                });
                //return false;
                //END OF TEST

                //appendPre(event.summary + event.location + ' (' + when + ')' + event.end.dateTime)
            }
        } else {
            appendPre('No upcoming events found.');
        }
    });
}
gapsButton.onclick = findGapsClick;
function findGapsClick(event) {
    $.ajax({
        url: findGap,
    }).done(function () {
        alert('Added');
    });
};

