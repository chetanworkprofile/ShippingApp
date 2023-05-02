mapboxgl.accessToken = 'pk.eyJ1Ijoiam9vc2hpIiwiYSI6ImNsaDRjeTBiazBqeG0zZ281enNzOXR4cjcifQ.eLxwYoRL5rhHhQxjv9mZkg';
//for checkpoints
function initialize() {
    const map = new mapboxgl.Map({
        container: document.getElementById("map"), // container ID
        style: 'mapbox://styles/mapbox/streets-v12', // style URL
        center: [78.9629, 20.5937], // starting position [lng, lat]
        zoom: 4, // starting zoom
    });
    // Add the control to the map.
    map.addControl(
        new MapboxGeocoder({
            accessToken: mapboxgl.accessToken,
            mapboxgl: mapboxgl
        })
    );
    var marker = new mapboxgl.Marker()
    map.on('click', function (e) {
        // handle click event here
        // get the coordinates of the clicked location;
        var longitude = e.lngLat.lng;
        var latitude = e.lngLat.lat;
        // do something with the coordinates, e.g. display them on the page
        // Set marker options.
        marker.remove();
        marker = new mapboxgl.Marker({
            color: "#b20000",
            draggable: true
        }).setLngLat([longitude, latitude])
            .addTo(map);
        //console.log(e);
        DotNet.invokeMethodAsync('ShippingClient', 'AddCoordinates', latitude, longitude);
    });
}

/*function init2() {
    //https://api.mapbox.com/directions/v5/mapbox/driving/-74.070358%2C40.920717%3B-73.406231%2C41.140664%3B-73.842321%2C40.878819%3B-73.839923%2C40.974825?alternatives=true&geometries=geojson&language=en&overview=simplified&steps=true&access_token=YOUR_MAPBOX_ACCESS_TOKEN
    const v = https://api.mapbox.com/directions/v5/mapbox/driving/-74.070358%2C40.920717%3B-73.406231%2C41.140664?alternatives=true&geometries=geojson&language=en&overview=simplified&steps=true&access_token=pk.eyJ1Ijoiam9vc2hpIiwiYSI6ImNsaDRjeTBiazBqeG0zZ281enNzOXR4cjcifQ.eLxwYoRL5rhHhQxjv9mZkg;
    const map = new mapboxgl.v;
}
*/

function mapinit2(_origin1, _origin2, _destination1, _destination2) {
    const map = new mapboxgl.Map({
        container: document.getElementById("map"), // container ID
        style: 'mapbox://styles/mapbox/streets-v12', // style URL
        center: [_origin1, _origin2], // starting position [lng, lat]
        zoom: 3, // starting zoom
    });
    var directions = new MapboxDirections({
        accessToken: mapboxgl.accessToken,
    });
    
    var origin = [_origin1, _origin2];
    var destination = [_destination1, _destination2];

    console.log(origin + " " + destination)

    directions.setOrigin(origin);
    directions.setDestination(destination);
    map.addControl(
        directions,
        'top-left'
    );
}