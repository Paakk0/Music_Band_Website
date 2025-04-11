document.addEventListener('DOMContentLoaded', function () {
    const titleFilter = document.getElementById('filter-title');
    const typeFilter = document.getElementById('filter-type');
    const likedFilter = document.getElementById('filter-liked');
    const songList = document.getElementById('song-list').children;

    function applyFilters() {
        Array.from(songList).forEach(row => {
            const title = row.children[1].textContent.toLowerCase();
            const type = row.dataset.type;
            const isLiked = row.dataset.liked === "true";

            const matchesTitle = title.includes(titleFilter.value.toLowerCase());
            const matchesType = typeFilter.value === "" || typeFilter.value === type;
            const matchesLiked = !likedFilter || !likedFilter.checked || isLiked;

            row.style.display = matchesTitle && matchesType && matchesLiked ? '' : 'none';
        });
    }


    titleFilter.addEventListener('input', applyFilters);
    typeFilter.addEventListener('change', applyFilters);
    likedFilter.addEventListener('change', applyFilters);
});
