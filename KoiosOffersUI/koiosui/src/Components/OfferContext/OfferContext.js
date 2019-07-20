import React, { createContext } from 'react';
import lodash from 'lodash';

const OfferContext = createContext({
  articleCollection: [],
  addToCollection: () => [],
  removeFromCollection: () => []
});

export class OfferProvider extends React.Component {
    state = {
        articleCollection: []
    }
    
    addToCollection = article => {
        OfferContext.articleCollection.push(article);
    }

    removeFromCollection = (article) => {
        let newCollection = lodash.remove(...this.state.articleCollection, value => 
            value.id === article.id
        )

        this.setState(
            articleCollection => {
                articleCollection = newCollection
                return articleCollection
            }
        )
    }
    render() {
        return (
            <OfferContext.Provider value={this.state}>
                {this.props.children}
            </OfferContext.Provider>
        );
  }
}

export const UserConsumer = UserContext.Consumer;