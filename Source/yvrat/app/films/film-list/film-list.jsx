import {connect} from 'react-redux';
import axios from 'axios';
import {actions, actionCreators} from '../../actions.jsx';
import ItemsList from '../../items-list/items-list.jsx';
import FilmItem from '../film-item/film-item.jsx';

export default connect(
    (state, ownProps) => ({
        ...ownProps,
        items: state.films.moviesListForm.visibleItems, 
        itemComponent: FilmItem,
        selectedId: state.films.moviesListForm.selectedId
    }), 
    (dispatch) => ({
        onClick: id => dispatch(actionCreators.watchMovie(id)),
        onSelect: id => dispatch(actionCreators.moviesListSelect(id)),
        onScroll: () => dispatch(actionCreators.moviesListLoadPage()),
        onToggleDescription: (id, expand) => dispatch(actionCreators.moviesListToggleDescription(id, expand))
    })
)(ItemsList);