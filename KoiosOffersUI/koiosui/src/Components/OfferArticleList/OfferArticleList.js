import React from 'react';
import * as Article from '../Article/Article.js';
import * as Offer from '../Offer/Offer.js';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import { exportDefaultSpecifier } from '@babel/types';
import ArticleElement from '../ArticleElement/ArticleElement.js';

export default class OfferArticleList extends React.Component{
    constructor(props) {
        super(props)
    
        this.state = {
             id: 0,
             selectedArticleId: 0,
             modal: false,
             articleCollection: []
        }

        this.toggle = this.toggle.bind(this);
        this.renderArticlesFromOffer = this.renderArticlesFromOffer.bind(this);
    }

    toggle() {
        this.setState(prevState => ({
          modal: !prevState.modal
        }));
    }

    async delete() {
        if(this.props.offerId === 0) {
            alert("Offer is not selected.");
        }

        else if(this.state.id === 0) {
            alert("Article is not selected.");
        }

        else {
            await Offer.deleteOfferArticle(this.props.offerId, this.state.id);
            this.toggle();
        }
    }

    async addToOffer(){
        
        if(this.props.offerId === 0) {
            alert("Offer is not selected.");
        }

        else if(this.state.selectedArticleId === 0) {
            alert("Article is not selected.");
        }
        
        else {
            await Offer.addOfferArticle(this.props.offerId, this.state.selectedArticleId);
        }
    }

    removeFromCollection(value) {
        this.state.articleCollection.splice(value, 1);
    }

    async renderArticlesFromOffer() {
        let list = await Offer.getOfferArticles(this.props.offerId);

        this.setState({
            articleCollection: list
        });

        // let rendered = Object.keys(list).map(key => {
        //     <ArticleElement 
        //         id={list[key].id} 
        //         name={list[key].name} 
        //         price={list[key].price}
        //     />
        // })
    }

    render() {
        return(
            <div id="offerList">
                <div className="listHeader">
                    <div id="selectArticle">
                        
                    </div>

                    <div id="addToOffer">
                        <Button color="primary" onClick={this.addToOffer}>Add article to offer</Button>
                    </div>

                    <div id="updateArticle">
                        <Button color="secondary">Update article</Button>
                    </div>

                    <div id="deleteArticle">
                        <Button color="secondary">Delete article</Button>
                    </div>

                    <div id="newArticle">
                        <Button color="secondary">Add article</Button>
                    </div>
                </div>

                <div>
                    {
                        this.renderArticlesFromOffer()
                    }
                </div>
            </div>
        ); 
    }
}
