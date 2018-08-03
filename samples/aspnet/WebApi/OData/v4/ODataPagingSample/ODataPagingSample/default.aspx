<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>Movies</title>
    <link rel="stylesheet" type="text/css" href="StyleSheet.css" />
</head>
<body>
    <h1>Movies</h1>

    Query Options: <input id="query" />
    <button onclick="runQueryClicked()">Run Query</button>
    <p id="currentPageDisplay">Current Page: <span data-bind="text: currentPage()"></span></p>
    <img src="Assets/previous.png" id="previousButton" onclick="previousButtonClicked()" data-bind="hidden: previousPages().length == 0" />
    <img src="Assets/next.png" id="nextButton" onclick="nextButtonClicked()" data-bind="hidden: nextPage() == null" />
    <div id="pageCounter" data-bind="hidden: totalPageNumber() == undefined">
        Page <span data-bind="text: currentPageNumber"></span>/<span data-bind="text: totalPageNumber"></span>
    </div>

    <br />

    <table>
        <colgroup>
            <col id="idCol" />
            <col id="titleCol" />
            <col id="genreCol" />
            <col id="releaseYearCol" />
            <col id="ratingCol" />
            <col id="popularityCol" />
        </colgroup>
        <thead>
            <tr>
                <th>ID</th>
                <th>Title</th>
                <th>Genre</th>
                <th>Release Year</th>
                <th>Rating</th>
                <th>Popularity</th>
            </tr>
        </thead>
        <tbody data-bind="foreach: movies">
            <tr>
                <td data-bind="text: ID"></td>
                <td data-bind="text: Title"></td>
                <td data-bind="text: Genre"></td>
                <td data-bind="text: ReleaseYear"></td>
                <td data-bind="text: Rating"></td>
                <td data-bind="text: Popularity"></td>
            </tr>
        </tbody>
    </table>
    <script src="Scripts/jquery-2.1.0.min.js"></script>
    <script src="Scripts/knockout-3.1.0.js"></script>
    <script src="Scripts/paging.js"></script>
</body>
</html>
