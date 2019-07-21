import React from 'react';
import * as Offer from '../Offer/Offer.js';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

export default class ArticleElement extends React.Component{
    constructor(props) {
        super(props)
    
        this.state = {
            id: 0
        }
    }

    delete = async () => {
        await Offer.deleteOfferArticle(props.offerId, this.state.id);
    }

    render() {
        return(
            <div className="articleElement">
                <div className="articleName">
                    {this.props.name}
                </div>

                <div className="articlePrice">
                    {this.props.price}
                </div>

                <div className="deleteFromOffer">
                    <Button color="warning" onClick={this.delete}>Remove from offer</Button>
                </div>
            </div>
        ); 
    }
}
