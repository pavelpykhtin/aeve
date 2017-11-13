import filmStorageReducer from './storage-reducer.jsx';
import moviesListReducer from './film-list/reducer.jsx';
import {combineReducers} from 'redux';

export default combineReducers({
    storage: filmStorageReducer,
    moviesListForm: moviesListReducer
});