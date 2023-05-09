mapboxgl.accessToken = 'pk.eyJ1Ijoiam9vc2hpIiwiYSI6ImNsaDRjeTBiazBqeG0zZ281enNzOXR4cjcifQ.eLxwYoRL5rhHhQxjv9mZkg';
//for adding checkpoints by admin map
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
    const middle = [(_origin1 + _destination1) / 2, (_origin2 + _destination2) / 2];
    const map = new mapboxgl.Map({
        container: document.getElementById("map"), // container ID
        style: 'mapbox://styles/mapbox/streets-v12', // style URL
        center: middle, // starting position [lng, lat]
        zoom: 8, // starting zoom
    });

    var url = `https://api.mapbox.com/directions/v5/mapbox/driving/${start[0]},${start[1]};${end[0]},${end[1]}?steps=true&geometries=geojson&access_token=${mapboxgl.accessToken}`;
    fetch(url)
        .then(response => response.json())
        .then(data => {
            console.log(data);
            var route = data.routes[0].geometry;
            var distance = data.routes[0].distance / 1000;
            var zoom = 8;
            if (distance < 50) {
                zoom = 10;
            }
            else if (distance < 150) {
                zoom = 8;
            }
            else if (distance < 500) {
                zoom = 6;
            }
            else{
                zoom = 4;
            }
            map.setZoom(zoom);
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

//function to create map for client to track shipment 
function shipmentHistory(lats, longis, count, shipmentStatus, shipmentLongitude, shipmentLatitude) {
    console.log(`${shipmentStatus} ${shipmentLongitude} ${shipmentLatitude}`);
    const start = [longis[0], lats[0]];
    const end = [longis[count - 1], lats[count - 1]];
    const middle = [(start[0] + end[0]) / 2, (start[1] + end[1]) / 2];
    const map = new mapboxgl.Map({
        container: document.getElementById("map"), // container ID
        style: 'mapbox://styles/mapbox/streets-v12', // style URL
        center: middle, // starting position [lng, lat]
        zoom: 8, // starting zoom
    });

    var str = "";
    for (var i = 0; i < count; i++) {
        if (i == (count - 1)) {
            str += `${longis[i]}%2C${lats[i]}`;
            break;
        }
        str += `${longis[i]}%2C${lats[i]}%3B`;
    }

    //https://api.mapbox.com/directions/v5/mapbox/driving/-74.248962%2C40.887627%3B-74.238293%2C40.841121%3B-74.119222%2C40.819609?alternatives=true&geometries=geojson&language=en&overview=simplified&steps=true&access_token=pk.eyJ1IjoicmFrZXNoa3VtYXIxOTk5IiwiYSI6ImNsaDRxczM2NDBub3Azbm81em1idmIwMDkifQ.jySmnyqMJg_hV45pHXcmNQ
    var url = `https://api.mapbox.com/directions/v5/mapbox/driving/${str}?alternatives=false&geometries=geojson&language=en&overview=simplified&steps=true&access_token=${mapboxgl.accessToken}`;

    const geojson = {
        'type': 'FeatureCollection',
        'features': [
            {
                'type': 'Feature',
                'geometry': {
                    'type': 'Point',
                    'coordinates': [shipmentLongitude, shipmentLatitude]
                },
                'properties': {
                    'title': shipmentStatus
                }
            }
        ]
    };

    fetch(url)
        .then(response => response.json())
        .then(data => {
            //console.log(data);
            var route = data.routes[0].geometry;
            var distance = data.routes[0].distance / 1000;
            var zoom = 8;
            if (distance < 50) {
                zoom = 10;
            }
            else if (distance < 150) {
                zoom = 8;
            }
            else if (distance < 500) {
                zoom = 6;
            }
            else {
                zoom = 4;
            }
            map.setZoom(zoom);
            map.on('load', () => {
                for (var i = 1; i < count - 1; i++) {
                    if (longis[i] != shipmentLongitude || lats[i] != shipmentLatitude) {
                        var marker = new mapboxgl.Marker({
                            color: "#c7a1ac",
                            draggable: false
                        }).setLngLat([longis[i], lats[i]])
                            .addTo(map);
                    }
                }

                if (start[0] != shipmentLongitude || start[1] != shipmentLatitude) {
                    var marker2 = new mapboxgl.Marker({
                        color: "#3887be",
                        draggable: false
                    }).setLngLat(start)
                        .addTo(map);
                }
                if (end[0] != shipmentLongitude || end[1] != shipmentLatitude) {
                    var marker2 = new mapboxgl.Marker({
                        color: "#f30",
                        draggable: false
                    }).setLngLat(end)
                        .addTo(map);
                }


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

                const el = document.createElement('div');
                el.className = 'marker';

                // make a marker for each feature and add it to the map
                new mapboxgl.Marker(el)
                    .setLngLat(geojson.features[0].geometry.coordinates)
                    .setPopup(
                        new mapboxgl.Popup({ offset: 25 }) // add popups
                            .setHTML(
                                `<h3>${geojson.features[0].properties.title}</h3>`
                            )
                    )
                    .addTo(map);
            });
        });
}

function createOrder(name, email, phone, address, orderId) {
    //var orderId = "order_LnS9siHznqfFkn"
    var options = {
        "name": "Shippi",
        "description": "New Shipment Order",
        "order_id": orderId,
        "image": "https://img.icons8.com/?size=512&id=18974&format=png",
        "prefill": {
            "name": name,
            "email": email,
            "contact": phone,
        },
        "notes": {
            "address": address
        },
        "theme": {
            "color": "#594AE2"
        }
    }
    // Boolean whether to show image inside a white frame. (default: true)

    options.handler = function (response) {
        DotNet.invokeMethodAsync('ShippingClient', 'VerifyResponseRazorpay', response.razorpay_payment_id, orderId, response.razorpay_signature);
        //document.getElementById('razorpay_payment_id').value = response.razorpay_payment_id;
        //document.getElementById('razorpay_order_id').value = orderId;
        //document.getElementById('razorpay_signature').value = response.razorpay_signature;
        //document.razorpayForm.submit();
        //return values to c# function from here
        // return 
    };
    options.modal = {
        ondismiss: function () {
            console.log("This code runs when the popup is closed");
        },
        // Boolean indicating whether pressing escape key
        // should close the checkout form. (default: true)
        escape: true,
        // Boolean indicating whether clicking translucent blank
        // space outside checkout form should close the form. (default: false)
        backdropclose: false
    };
    var rzp = new Razorpay(options);
    rzp.open();
}

function GetResult() {
    return res;
}




/*

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
*/