import {actions, actionCreators} from '../actions.jsx';

const initialState = {
    items: [],
    itemsById: {}
};

function onDetailsLoadSuccess(state, action){
    const newItems = state.items
        .map(i => i.id == action.details.id ? {...i, description: action.details.description} : i);
    const newItemsById = newItems.reduce((s,f) => (s[f.id] = f, s), {});
    return {
        ...state, 
        items: newItems,
        itemsById: newItemsById
    };
}

export default function StorageReducer(state = initialState, action){
    switch(action.type){
        case actions.MOVIES_FETCH_SUCCESS:
            const fetchedItems = action.films.filter(x => !x.isWatched);
            const newItems = state.items
                .filter(x => !fetchedItems.some(f => f.id == x.id))
                .concat(fetchedItems);
            
            const newItemsById = fetchedItems.reduce((s, f) => (s[f.id] = f, s), {});

            return {
                items: newItems, 
                itemsById: {...state.itemsById, ...newItemsById}
            };

        case actions.MOVIES_WATCH_SUCCESS:
            window.location = action.movieUrl;
            return state;

        case actions.MOVIES_DETAILS_LOAD_SUCCESS:
            return onDetailsLoadSuccess(state, action);

        default:
            return state;
    }
}