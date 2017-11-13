import {actions, actionCreators} from '../../actions.jsx';

const initialState = {
    selectedId: null,
    visibleItems: [],
    page: 0,
    isLoadingPage: false
};

function onSetDescription(state, action){
    const visibleItems = state.visibleItems
        .map(x => x.id == action.details.id ? {...x, description: action.details.description} : x);

    return {...state, visibleItems: visibleItems};
}

export default function MovieListFormReducer(state = initialState, action){
    switch(action.type){
        case actions.MOVIES_DETAILS_LOAD_SUCCESS:
            return onSetDescription(state, action);
        
        case actions.MOVIES_LIST_SELECT:
            return {...state, selectedId: action.id};

        case actions.MOVIES_LIST_LOAD_PAGE:
            return {...state, isLoadingPage: true};

        case actions.MOVIES_LIST_LOAD_PAGE_SUCCESS:
            return {...state, page: action.page, visibleItems: action.visibleItems, isLoadingPage: false};

        case actions.MOVIES_LIST_SET_EXPANDED:
            return {
                ...state, 
                visibleItems: state.visibleItems.map(x => ({...x, isExpanded: action.isExpanded && x.id == action.id}))
            };
            
        default:
            return state;
    }
}