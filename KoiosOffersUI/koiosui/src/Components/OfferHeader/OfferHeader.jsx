import React, { createContext } from 'react';
import { Button, Input, Modal, ModalHeader, ModalBody, ModalFooter } from 'reactstrap';
import Autocomplete from 'react-autocomplete';
import axios from 'axios';
import * as Offer from '../Offer/Offer.js';
import './OfferHeader.css';
import linq from 'linq';
import lodash from 'lodash';
import { OfferContext, OfferConsumer, OfferProvider } from '../OfferContext/OfferContext';

let tmpId = 0;

export default class OfferHeader extends React.Component {
    
    constructor(props) {
        super(props)
        this.state = {
             id: 0,
             modal: false,
             offerId: null,
             offerNumner: null,
             offerCreatedAt: "",
             value: '',
             suggestions: [],
             article: {
                 id: 1,
                 name: "Article1",
                 unitPrice: 500
             },
            testArticleList: []
        }

        this.onTextChange = this.onTextChange.bind(this);
        this.onSelect = this.onSelect.bind(this);
        this.getOffer = this.getOffer.bind(this);
    }

    removeFromOffer = ({...article}, {...key}) => {
        let newValues = lodash.remove(...this.state.suggestions, value => 
            value.id === article.id
        )

        this.setState(
            testArticleList => {
                testArticleList = newValues
                return testArticleList
            }
        )
    }

    onTextChange = async (e) => {
        this.setState({
          offerId: e.target.value
        });
    };

    onSelect(val) {
        this.setState({
            value: val
        });
    }

    addToOffer() {
        let article = {
            id: tmpId,
            name: "Kurac",
            unitPrice: 500
        }

        tmpId++;
    }

    async getOffer() {
        // let result;

        // let offerNumber = this.state.offerId;

        // let url = "http://localhost:59189/api/Offer/GetOfferByOfferNumber?offerNumber=" + offerNumber;
        
        // await axios.get(url).then(response => {
        //     result = response.data;
        // }).catch(error => {
        //     throw -1;
        // })

        // if(result.Number === null) {
        //     alert("There is no offer with offer number: " + this.state.offerNumber);
        // }

        // this.setState({
        //     offerCreatedAt: result.Number
        // });

        // this.setState({
        //     offerCreatedAt: result.CreatedAt
        // });

        // this.state.testArticleList.push(this.state.article);
        // this.setState({ testArticleList: [...this.state.testArticleList, this.state.article]});
        // this.render();
        // console.log(this.state.testArticleList);
        // this.renderArticle()
    }

    async getData(searchText) {
        let _this = this;
        console.log("get data");
        let result;

        let url = 'http://localhost:59189/api/Offer/GetPaginated?offerNumber=' + searchText
            + '&skip=0&take=10';

        console.log(url);
        await axios.get(url)
            .then(response => {
                result = response.data;
            }).catch(error => {
                console.log(error);
                throw -1;
            });

        _this.setState({
            suggestions: result
        });
    }

    renderItem(item, isHighlighted){
        return (
            <div style={{ background: isHighlighted ? 'lightgray' : 'white' }}>
                {item.Number}
            </div>   
        ); 
    }

    getItemValue(item){
        return item.value;
    }

    componentWillMount() {
        {console.log("b")}
    }

    render() {
        return(
            <OfferContext.Consumer>
                {
                    (value) => {
                        return(
                            <div>
                                <div id="offerHeader">
                                    <div id="headerTop">
                                        <div id="offerSuggestion" className="headerTopElement">
                                            <Input onChange={this.onTextChange} placeholder="enter offer id"/>
                                        </div>
        
                                        <div id="newOffer" className="headerTopElement buttonSpace">
                                            <Button onClick={this.getOffer} className="buttonStyle">Search offer</Button>
                                        </div>
        
                                        <div id="newOffer" className="headerTopElement">
                                            <Button className="buttonStyle">Create offer</Button>
                                        </div>
                                    </div>
        
                                    <div id="headerBottom">
                                        <div id="offerNumber">
                                            Offer number: {this.state.offerNumber}
                                        </div>
        
                                        <div id="offerDateTime">
                                            Offer created at: {this.state.offerCreatedAt}
                                        </div>
                                    </div>
                                </div>
        
                                <div id="articlesHeader" className="headerBottomStyle">
                                    <div className="articleMiddle">
                                        <div id="selectArticle" className="buttonBottomStyle">
                                            <Autocomplete
                                                getItemValue={this.getItemValue}
                                                items={value.suggestions}
                                                renderItem={this.renderItem}
                                                value={this.state.value}
                                                onChange={this.onChange}
                                                onSelect={this.onSelect}
                                            />
                                        </div>
        
                                        <div id="addToOffer" className="buttonBottomStyle buttonSpace">
                                            <Button onClick={value.addToCollection}>Add to offer</Button>
                                        </div>
        
                                        <div id="updateArticle" className="buttonBottomStyle buttonSpace">
                                            <Button>Change article</Button>
                                        </div>
        
                                        <div id="deleteArticle" className="buttonBottomStyle buttonSpace">
                                            <Button>Delete article</Button>
                                        </div>
        
                                        <div id="createArticle" className="buttonBottomStyle">
                                            <Button>New article</Button>
                                        </div>
                                    </div>
                                </div>

                                <div id="articleList1">
                                    {
                                        Object(value.articleCollection).map((article, key) => {
                                            return(
                                                value.renderArticle(article, key)
                                            )
                                        })
                                    }
                                </div>
                            </div>
                        );
                    }
                }
            </OfferContext.Consumer>
        ); 
    }
}

OfferHeader.contextType = OfferContext;
