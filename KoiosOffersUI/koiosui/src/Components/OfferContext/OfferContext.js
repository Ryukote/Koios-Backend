import React, { createContext } from 'react';
import { Button, Input, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import lodash from 'lodash';

export const OfferContext = createContext({
  articleCollection: [],
  addToCollection: () => [],
  removeFromCollection: () => [],
  renderArticle(value, key) {},
  returnCollection: () => {},
  suggestions: []
});

export class OfferProvider extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            articleCollection: [],
            addToCollection: this.addToCollection.bind(this),
            returnCollection: this.returnCollection.bind(this),
            renderArticle: this.renderArticle.bind(this),
            suggestions: []
        }

        this.addToCollection = this.addToCollection.bind(this);
        this.removeFromCollection = this.removeFromCollection.bind(this);
        this.returnCollection = this.returnCollection.bind(this);
        this.renderArticle = this.renderArticle.bind(this);
    }
    
    
    addToCollection = (article) => {
        // debugger;
        let tmpArticle = {
            id: 1,
            name: "Article",
            unitPrice: 300
        }

        let tmpArray = this.state.articleCollection.slice();

        // Object(this.state.articleCollection).map((value, key) => {
        //     tmpArray.push(value);
        // });

        tmpArray.push(tmpArticle);

        this.setState({
            articleCollection: tmpArray
        })



        // console.log(tmpArray);

        // this.state.articleCollection.push(tmpArticle);
        console.log(this.state.articleCollection);
        // console.log("Završio");
    }

    removeFromCollection = (article) => {
        let tmpArray = this.state.articleCollection;

        tmpArray = lodash.remove(tmpArray, value => 
            value.id === article.id
        )

        console.log(tmpArray);

        this.setState({
            articleCollection: tmpArray
        })

        // this.setState(
        //     articleCollection => {
        //         articleCollection = newCollection
        //         return articleCollection
        //     }
        // )
    }

    returnCollection = () => {
        return this.state.articleCollection;
    }

    renderArticle(value, key) {
        // debugger;
        console.log("Krankšvester");
        if(value != null){
            return(
                <div key={key}>
                    <div>
                        {"Article id: " + value.id}
                    </div>
    
                    <div>
                        {"Article name: " + value.name}
                    </div>
    
                    <div>
                        {"Article price: " + value.unitPrice}
                    </div>
    
                    <div>
                        <Button color="danger" onClick={() => this.removeFromCollection({...key})}>Remove from offer</Button>
                    </div>
                </div>
            );
        }
        else{
                return(
                    <div></div>
                );
        }
    }

    render() {
        return (
            <OfferContext.Provider value={{...this.state}}>
                {{...this.props.children}}
            </OfferContext.Provider>
        );
    }
}

export const OfferConsumer = OfferContext.Consumer;