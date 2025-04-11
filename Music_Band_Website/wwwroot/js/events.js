document.addEventListener('DOMContentLoaded', function () {
    const cityFilter = document.getElementById('event-search-city');
    const locationFilter = document.getElementById('event-search-location');
    const dateFilter = document.getElementById('event-search-date');
    const eventList = document.getElementById('event-list').children;

    function applyFilters() {
        const cityValue = cityFilter.value.toLowerCase();
        const locationValue = locationFilter.value.toLowerCase();
        const dateValue = dateFilter.value;

        Array.from(eventList).forEach(eventCard => {
            const eventCity = eventCard.dataset.name.toLowerCase();
            const eventLocation = eventCard.dataset.location.toLowerCase();
            const eventDate = eventCard.dataset.date; // Directly use the full date from dataset

            const matchesCity = cityValue === "" || eventCity.includes(cityValue);
            const matchesLocation = locationValue === "" || eventLocation.includes(locationValue);
            const matchesDate = dateValue === "" || eventDate === dateValue;

            eventCard.style.display = matchesCity && matchesLocation && matchesDate ? '' : 'none';
        });
    }

    cityFilter.addEventListener('input', applyFilters);
    locationFilter.addEventListener('input', applyFilters);
    dateFilter.addEventListener('change', applyFilters);
});
