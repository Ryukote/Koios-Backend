import React from 'react';
// import Autosuggest from 'react-autosuggest';
import * as Article from '../Article/Article.js';
import * as Offer from '../Offer/Offer.js';
import { Button, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';

export default class ArticleElement extends React.Component{
    constructor(props) {
        super(props)
    
        this.state = {
             id: 0,
             modal: false
            //  suggestions: []
        }

        this.toggle = this.toggle.bind(this);
    }

    // onChange = (event, { newValue }) => {
    //     this.setState({
    //       value: newValue
    //     });
    //   };

    // getSuggestions = async (value) => {
    //     this.setState({
    //         suggestions: await Article.getSuggestionsByName(value, 0, 10)
    //     });
    // };

    // onSuggestionsClearRequested = () => {
    //     this.setState({
    //       suggestions: []
    //     });
    // };

    // getSuggestionValue = suggestion => suggestion.name;

    // renderSuggestion = suggestion => (
    //     <div>
    //       {suggestion.name}
    //     </div>
    // );

    // inputProps = {
    //     placeholder: 'Type article name',
    //     value,
    //     onChange: this.onChange
    //   };

    toggle() {
        this.setState(prevState => ({
          modal: !prevState.modal
        }));
    }

    async delete() {
        await Offer.deleteOfferArticle(props.offerId, this.state.id);
        this.toggle();
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
                    <Button color="warning" onClick={this.toggle}>Remove from offer</Button>
                </div>

                <Modal isOpen={this.state.modal} toggle={this.toggle} className={this.props.className}>
                    <ModalHeader toggle={this.toggle}>Remove article from offer</ModalHeader>
                    <ModalBody>
                        Are you sure you want to remove this article from this offer?
                    </ModalBody>
                    <ModalFooter>
                        <Button color="primary" onClick={this.delete}>I am sure</Button>{' '}
                        <Button color="secondary" onClick={this.toggle}>Cancel</Button>
                    </ModalFooter>
                </Modal>

                
            </div>
        ); 
    }
}
