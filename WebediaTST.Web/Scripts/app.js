
$(document).ready(function () {
    var n = 3;
    $.ajax({
        url: "api/Events/",
        type : 'GET',
        async: false,
        dataType: 'json',
        data: n,
        success: function (data) {
            if (data.length > 0) {
                for (var i = 0, len = data.length; i < len; i++) {
                    $('#Events tr:last').after('<tr><td>' + data.EventType + '</td><td>' + data.Path + '</td><td>' + data.Date + '</td></tr>');
                }
            }
            else {
                $('#Events').html('Pas de nouveaux événements....!');
            }
        },
        error: function (resultat, statut, erreur) {
            console.log("Erreur survenue dans le chargement des données, vérifier l'url de service web...");
            $('#Events').html('Pas de nouveaux événements....!');
        }
    });

});