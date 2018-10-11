Vue.component("clash-clan-info",
    {
        props: {
            info: { type: Object },
            active: { type: Boolean, default: false }
        },
        template: `
<div class="block about" v-bind:class="{ active: active }">
    <div>
        <img v-bind:src="info.badgeUrls.small" alt="">
    </div>
    <div>
        <div>Всего очков: {{info.clanPoints}} / {{info.clanVersusPoints}}</div>
        <div>Серия побед: {{info.warWinStreak}}</div>
        <div>Выиграно войн: {{info.warWins}}</div>
        <div>Проиграно войн: {{info.warLosses}}</div>
        <div>Ничья: {{info.warTies}}</div>
        <div>Побед &divide; поражений: {{(info.warLosses === 0 ? 0 : info.warWins / info.warLosses).toFixed(3)}}</div>
        <div>Публичный варлог: {{info.isWarLogPublic ? "Да" : "Нет"}}</div>
        <div>Необходимо трофеев: {{info.requiredTrophies}}</div>
        <div>Расположение клана: {{info.location.name}}</div>
        <div>Соклановцы: {{info.members}}/50</div>
    </div>
</div>

`
    });

Vue.component("clash-clan-moto",
    {
        props: {
            description: {type: String},
            active: { type: Boolean, default: false }
        },
        template: `
<div class="block" v-bind:class="{ active: active }">
    <div>{{description}}</div>
</div>
`
    });

Vue.component("clash-clan-donation-details", {
    props: {
        item: { type: Object },
        totalDonation: { type: Number }
    },
    computed: {
        openPlayerProfileHref: function () {
            return `https://link.clashofclans.com/?action=OpenPlayerProfile&tag=${this.item.tag}`;
        }
    },
    template: `
<div>
    <div style="font-size: larger;">
        <a v-bind:href="openPlayerProfileHref">
            <img v-bind:src="item.league.iconUrls.small" width="25" height="25"/>
            {{item.name}} ({{item.expLevel}})
        </a>
    </div>
    <div>
        <span>Пожертвовал: </span>{{item.donations}}
    </div>
    <div>
        <span>Получил: </span>{{item.donationsReceived}}
    </div>
    <div>
        <span>Вклад: </span>{{((item.donations / totalDonation)*100).toFixed(1)}}%
    </div>
</div>
`
});

Vue.component("clash-clan-donation",
    {
        props: {
            clanDetails: { type: Object },
            active: { type: Boolean, default: false }
        },
        computed: {
            totalDonation: function () {
                return this.clanDetails.memberList.reduce((d, m) => m.donations + d, 0);
            },
            orderedMemberList: function () {
                return _.orderBy(this.clanDetails.memberList, "donations", "desc");
            }
        },
        template: `
<div class="block" v-bind:class="{ active: active }">
    <h2>Донат</h2>
    <ol>
        <li class="user-details" v-for="item in orderedMemberList">
            <clash-clan-donation-details v-bind:item="item" v-bind:totalDonation="totalDonation"></clash-clan-donation-details>
        </li>
    </ol>
</div>
`
    });

Vue.component("clash-warlog-item",
    {
        props: {
            item: { type: Object }
        },
        computed: {
            background: function () {
                switch (this.item.result) {
                    case "win":
                        return "background-image: linear-gradient(#BDD38C, #A5C76B)";
                    case "lose":
                        return "background-image: linear-gradient(#C88083, #BD696B)";
                    default :
                        return "background-image: linear-gradient(#D1D0C7, #C2C2B7)";
                }
            },
            openClanHref: function() {
                return `https://link.clashofclans.com/?action=OpenClanProfile&tag=${this.item.opponent.tag}`;
            }
        },
        template: `
<div class="row user-details-row" v-bind:style="background">
    <span style="flex: 1; display: flex; justify-content: flex-end; align-items: center;">
        <div style="display: flex; flex-wrap: wrap; justify-content: flex-end; align-items: center;">
            <span style="font-size: 9px">Optimus Gang</span>
            <span style="margin-left: 5px">{{item.clan.destructionPercentage.toFixed(2)}}%</span>
        </div>
        <span>
            <img v-bind:src="item.clan.badgeUrls.small" width="40">
        </span>
    </span>
    <span style="flex:1; display: flex; justify-content: flex-start; align-items: center;">
        <span>
            <img v-bind:src="item.opponent.badgeUrls.small" width="40">
        </span>
        <div style="display: flex; flex-wrap: wrap; justify-content: flex-start; align-items: center;">
            <span style="margin-right: 5px">{{item.opponent.destructionPercentage.toFixed(2)}}%</span>
            <a v-bind:href="openClanHref" style="font-size: 9px">{{item.opponent.name}}</a>
        </div>
    </span>
</div>
`
    });

Vue.component("clash-warlog",
    {
        props: {
            warlog: { type: Object },
            active: { type: Boolean, default: false }
        },
        template: `
<div class="block" v-bind:class="{ active: active }">
    <h2>Ход войны</h2>
    <ul>
        <li class="user-details" v-for="item in warlog.items">
            <clash-warlog-item v-bind:item="item"></clash-warlog-item>
        </li>
    </ul>
</div>
`
    });

var app = new Vue({
    el: "#app",
    created() {
        this.fetchData();
    },
    data() {
        return {
            clan: {
                warWins: 0,
                warLosses: 0,
                badgeUrls: { small: null },
                location: { name: null },
                description: ""
            },
            warlog: {
            },
            clanInfoActive: false,
            warlogActive: false
        }
    },
    methods: {
        fetchData() {
            fetch("api/clash/clan").then(r => r.json().then(t => {
                this.clan = t;
                this.clanInfoActive = true;
            }));

            fetch("api/clash/warlog").then(r => r.json().then(t => {
                this.warlog = t;
                this.warlogActive = true;
            }));
        }
    },
    template: `
<div>
<clash-clan-info v-bind:info="clan" v-bind:active="clanInfoActive"></clash-clan-info>
<clash-clan-moto v-bind:description="clan.description" v-bind:active="clanInfoActive"></clash-clan-moto>
<clash-clan-donation v-bind:clanDetails="clan" v-bind:active="clanInfoActive"></clash-clan-donation>
<clash-warlog v-bind:warlog="warlog" v-bind:active="warlogActive"></clash-warlog>
</div>
`
})