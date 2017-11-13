import React from 'react';
import ReactDom from 'react-dom';
import {List, ListItem} from 'react-toolbox/lib/list';
import './items-list.less';

class ItemsList extends React.Component {
    render() {
        const props = this.props;
        const Item = props.itemComponent;
        const items = props
            .items
            .map((p, i) => ({
                ...p,
                ref: p.id == props.selectedId ? 'selectedItem' : null,
                isSelected: p.id == props.selectedId || (i == 0 && !props.selectedId),
                onClick: props.onClick,
                onSelect: props.onSelect,
                onToggleDescription: props.onToggleDescription
            }))
            .map(p => <Item {...p} key={p.id}/>);

        return (
            <div className={`items-list ${props.className}`} 
                onScroll={this.onScroll()}>
                {items}
            </div>
        );
    }

    componentDidUpdate(prevProps) {
        if (this.props.selectedId !== prevProps.selectedId) {
            this.ensureSelectedItemVisible();
        }
    }

    ensureSelectedItemVisible() {
        const itemComponent = this.refs.selectedItem;
        if (itemComponent) {
            const domNode = ReactDom.findDOMNode(itemComponent);
            this.scrollElementIntoViewIfNeeded(domNode);
        }
    }

    scrollElementIntoViewIfNeeded(domNode) {
        domNode.scrollIntoViewIfNeeded(false);
    }

    onScroll() {
        return e => {
            if(e.target.scrollHeight - e.target.clientHeight - e.target.scrollTop > 400) return;
            
            this.props.onScroll();
        };
    }
}

export default ItemsList;