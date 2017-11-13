import React from 'react';
import {connect} from 'react-redux';
import {ListItem} from 'react-toolbox/lib/list';
import {actions, actionCreators} from '../../actions';
import './film-item.less';

class FilmItem extends React.Component{
    render(){
        const props = this.props;
        const className = `film-item ${props.isSelected ? 'film-item_selected' : ''} ${props.isExpanded && 'film-item_expanded'}`;
    
        return <div className={className}
                    key={props.id} 
                    onClick={this.onClick(props)}>
                    <img className='film-item__image' src={props.image}></img>
                    <input type='radio' 
                        onClick={this.onClick(props)} onKeyDown={this.onKeyDown(props)}
                        onFocus={this.onFocus(props)}
                        className='film-item__handler' 
                        autoFocus={props.isSelected}/>
                    <div className='film-item__info'>
                        <div className='film-item__name'>{props.name}</div>                    
                        <div className='film-item__description'>{props.description}</div>
                    </div>
                    <div className='film-item__actions'>
                        <div className='film-item__expander' onClick={this.expand(props)}></div>
                    </div>
                </div>;
    }

    onFocus(props){
        return () => props.onSelect(props.id);
    }
    
    onClick (props) {
        return e => {
            if(e.screenX == 0) return;
        
            props.onClick(props.id);
        };
    }
    
    onKeyDown(props){
        return e => {
            switch(e.keyCode){
                case 13:
                    props.onClick(props.id);
                    break;
                case 49:
                    props.onToggleDescription(props.id, !props.isExpanded);
                    break;
            }
        };
    }

    expand(props){
        return e => {
            e.stopPropagation();
            props.onToggleDescription(props.id, !props.isExpanded);        
        }
    }
}

export default FilmItem;