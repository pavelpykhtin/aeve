import {createStore, applyMiddleware} from 'redux';
import {render} from 'react-dom';
import {Provider, connect} from 'react-redux';
import thunk from 'redux-thunk';
import logger from 'redux-logger';
import React from 'react';
import axios from 'axios';
import AppBar from 'react-toolbox/lib/app_bar';
import './index.less';
import FilmsList from './films/film-list/film-list.jsx';
import CoreReducer from './core-reducer.jsx';
import {actions, actionCreators} from './actions.jsx';

axios.defaults.baseURL = 'http://docker-test-107h8q8b.cloudapp.net:9000/api';

const store = createStore(CoreReducer, applyMiddleware(thunk, logger));

store.dispatch(actionCreators.moviesListLoadPage(1, store.getState()));

render(
    <div className="page">
    <AppBar title='TVProxy v0.0.5' className="page__header"></AppBar>
    <Provider store={store}>
        <FilmsList className='page__content'></FilmsList>
    </Provider>
    <div className='debug'></div>
</div>, document.getElementById("root"));