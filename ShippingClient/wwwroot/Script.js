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

// for driver to do location tracking
function trackOnOriginDest(_origin1, _origin2, _destination1, _destination2) {
    const start = [_origin1, _origin2];
    const end = [_destination1, _destination2];

    const map = new mapboxgl.Map({
        container: document.getElementById("map"), // container ID
        style: 'mapbox://styles/mapbox/streets-v12', // style URL
        center: start, // starting position [lng, lat]
        zoom: 6, // starting zoom
    });

    var url = `https://api.mapbox.com/directions/v5/mapbox/driving/${start[0]},${start[1]};${end[0]},${end[1]}?steps=true&geometries=geojson&access_token=${mapboxgl.accessToken}`;
    fetch(url)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            var route = data.routes[0].geometry;
            map.on('load', () => {

                var marker = new mapboxgl.Marker({
                    color: "#3887be",
                    draggable: false
                }).setLngLat(start)
                    .addTo(map);

                var marker2 = new mapboxgl.Marker({
                    color: "#f30",
                    draggable: false
                }).setLngLat(end)
                    .addTo(map);

                map.addSource('route', {
                    'type': 'geojson',
                    'data': {
                        'type': 'Feature',
                        'properties': {},
                        'geometry': route
                    }
                });
                map.addLayer({
                    'id': 'route',
                    'type': 'line',
                    'source': 'route',
                    'layout': {
                        'line-join': 'round',
                        'line-cap': 'round'
                    },
                    'paint': {
                        'line-color': '#4D58B2',
                        'line-width': 6
                    }
                });
            });
        });
    /*const map = new mapboxgl.Map({
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
    )*/;
}



function funcInit() {
    const start = [-122.662323, 45.523751];
    const end = [-120.662323, 46.023751];

    const map = new mapboxgl.Map({
        container: document.getElementById("map"), // container ID
        style: 'mapbox://styles/mapbox/streets-v12', // style URL
        center: start, // starting position [lng, lat]
        zoom: 6, // starting zoom
    });
    
    
    var url = `https://api.mapbox.com/directions/v5/mapbox/driving/${start[0]},${start[1]};${end[0]},${end[1]}?steps=true&geometries=geojson&access_token=${mapboxgl.accessToken}`;

    fetch(url)
        .then(response => response.json())
        .then(data => {
            console.log(data)
            var route = data.routes[0].geometry;


    map.on('load', () => {
        
        var marker = new mapboxgl.Marker({
            color: "#3887be",
            draggable: false
        }).setLngLat(start)
            .addTo(map);

        var marker2 = new mapboxgl.Marker({
            color: "#f30",
            draggable: false
        }).setLngLat(end)
            .addTo(map);

        map.addSource('route', {
            'type': 'geojson',
            'data': {
                'type': 'Feature',
                'properties': {},
                'geometry': route
            }
        });
        map.addLayer({
            'id': 'route',
            'type': 'line',
            'source': 'route',
            'layout': {
                'line-join': 'round',
                'line-cap': 'round'
            },
            'paint': {
                'line-color': '#4D58B2',
                'line-width': 6
            }
        });
    });
    });

}
