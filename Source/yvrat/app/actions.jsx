import axios from 'axios';
import fuckCors from './fuckCors';

const aldrusBaseAddress = 'http://docker-test-107h8q8b.cloudapp.net:9001/api';
const PageSize = 60;

const actions = {
    MOVIES_FETCH: 'MOVIES_FETCH',
    MOVIES_FETCH_SUCCESS: 'MOVIES_FETCH_SUCCESS',
    MOVIES_WATCH: 'MOVIES_WATCH',
    MOVIES_WATCH_SUCCESS: 'MOVIES_WATCH_SUCCESS',
    MOVIES_DETAILS_LOAD_SUCCESS: 'MOVIES_DETAILS_LOAD_SUCCESS',
    MOVIES_LIST_SELECT: 'MOVIES_LIST_SELECT',
    MOVIES_LIST_LOAD_PAGE: 'MOVIES_LIST_LOAD_PAGE',
    MOVIES_LIST_LOAD_PAGE_SUCCESS: 'MOVIES_LIST_LOAD_PAGE_SUCCESS',
    MOVIES_LIST_TOGGLE_DESCRIPTION: 'MOVIES_LIST_TOGGLE_DESCRIPTION',
    MOVIES_LIST_SET_EXPANDED: 'MOVIES_LIST_SET_EXPANDED'
};

function fetchMovies(page) {
    return dispatch => {
        dispatch({type: actions.MOVIES_FETCH, page});
        axios
            .get(`movies?pageIndex=${page - 1}`, {crossDomain: true})
            .then(response => dispatch(fetchMoviesSuccess(response.data)));
    }
}

function fetchMoviesSuccess(films) {
    return dispatch => dispatch({type: actions.MOVIES_FETCH_SUCCESS, films});
}

function watchMovie(id) {
    return (dispatch, getState) => {
        const movie = getState().films.storage.itemsById[id];
        console.log(movie);
        dispatch({type: actions.MOVIES_WATCH, id})
        fuckCors
            .get(`https://zona.mobi/ajax/video/${movie.externalId}`)
            .then(data => dispatch(watchMovieSuccess(data)));
    }
}

function watchMovieSuccess(movieUrl) {
    return dispatch => dispatch({type: actions.MOVIES_WATCH_SUCCESS, movieUrl});
}

function moviesListSelect(id) {
    return dispatch => dispatch({type: actions.MOVIES_LIST_SELECT, id: id});
}

function moviesListLoadPage() {
    return (dispatch, getState) => {
        if(getState().films.moviesListForm.isLoadingPage) return;
        
        const page = getState().films.moviesListForm.page + 1;

        dispatch({type: actions.MOVIES_LIST_LOAD_PAGE, page});

        let items = getState().films.storage.items;
        if (items.length > page * PageSize) {
            dispatch(moviesListLoadPageSuccess(page))
        } else {
            dispatch({type: actions.MOVIES_FETCH, page});
            axios
                .get(`movies?pageIndex=${page - 1}`, {crossDomain: true})
                .then(response => dispatch(fetchMoviesSuccess(response.data)))
                .then(() => dispatch(moviesListLoadPageSuccess(page)));
        }
    }
}

function moviesListLoadPageSuccess(page){
    return (dispatch, getState) => {
        const visibleItems = getState().films.storage.items.slice(0, page * PageSize);
        dispatch({type: actions.MOVIES_LIST_LOAD_PAGE_SUCCESS, page, visibleItems});
    }
}

function moviesListToggleDescription(id, expand){
    return (dispatch, getState) => {
        const item = getState().films.storage.itemsById[id];
        dispatch({type: actions.MOVIES_LIST_TOGGLE_DESCRIPTION});
        
        if(item.description || !expand) {
            dispatch(moviesListSetExpanded(item.id, expand));
            return;
        }

        axios.get(`movies/${item.nameId}/details`, {crossDomain: true})
            .then(response => dispatch(moviesDetailsLoadSuccess(response.data)))
            .then(() => dispatch(moviesListSetExpanded(item.id, expand)));
    }
}

function moviesDetailsLoadSuccess(details){
    return dispatch => {
        console.log({type: actions.MOVIES_DETAILS_LOAD_SUCCESS, details });
        return dispatch({type: actions.MOVIES_DETAILS_LOAD_SUCCESS, details })
    }; 
}

function moviesListSetExpanded(id, isExpanded){
    return dispatch => dispatch({type: actions.MOVIES_LIST_SET_EXPANDED, isExpanded, id});
}
    
const actionCreators = {
    fetchMovies,
    fetchMoviesSuccess,
    watchMovie,
    watchMovieSuccess,
    moviesDetailsLoadSuccess,
    moviesListSelect,
    moviesListLoadPage,
    moviesListLoadPageSuccess,
    moviesListToggleDescription,
    moviesListSetExpanded
};

export {actions, actionCreators};